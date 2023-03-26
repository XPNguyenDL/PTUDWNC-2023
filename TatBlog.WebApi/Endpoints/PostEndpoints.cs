using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Media;
using TatBlog.WebApi.Models.PostModel;
using TatBlog.WebApi.Validations;

namespace TatBlog.WebApi.Endpoints;

public static class PostEndpoints
{
    public static WebApplication MaPostEndpoints(
        this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/posts");

        routeGroupBuilder.MapGet("/", GetPost)
            .WithName("GetPost")
            .Produces<PaginationResult<PostDto>>();

        routeGroupBuilder.MapGet("/featured/{limit:int}", GetFeaturedPost)
            .WithName("GetFeaturedPost")
            .Produces<IList<PostDto>>();

        routeGroupBuilder.MapGet("/random/{limit:int}", GetRandomPost)
            .WithName("GetRandomPost")
            .Produces<IList<PostDto>>();

        routeGroupBuilder.MapGet("/archives/{month:int}", GetArchives)
	        .WithName("GetArchives")
	        .Produces<IList<MonthlyPostCountItem>>();

        routeGroupBuilder.MapGet("/{id:guid}", GetPostById)
            .WithName("GetPostById")
            .Produces<PostDetail>()
            .Produces(404);

        routeGroupBuilder.MapGet("/byslug/{slug:regex(^[a-z0-9_-]+$)}", GetPostBySlug)
            .WithName("GetPostBySlug")
            .Produces<PaginationResult<PostDetail>>()
			.Produces(404);

        routeGroupBuilder.MapPost("/", AddPost)
            .WithName("AddPost")
            .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
			.Produces(201)
            .Produces(400)
            .Produces(409);

        routeGroupBuilder.MapPut("/{id:guid}", UpdatePost)
            .WithName("UpdatePost")
            .AddEndpointFilter<ValidatorFilter<PostEditModel>>()
            .Produces(204)
            .Produces(400)
            .Produces(409);

        routeGroupBuilder.MapGet("/TogglePost/{id:guid}", TogglePublicStatus)
	        .WithName("TogglePublicStatus")
	        .Produces(204)
	        .Produces(404);

		routeGroupBuilder.MapPost("/{id:guid}/picture", SetPostPicture)
	        .WithName("SetPostPicture")
	        .Accepts<IFormFile>("multipart/form-data")
	        .Produces<string>()
	        .Produces(400);

        routeGroupBuilder.MapDelete("/{id:guid}", DeletePost)
            .WithName("DeletePost")
            .Produces(204)
            .Produces(404);

        return app;
    }

    private static async Task<IResult> GetPost(
        [AsParameters] PostFilterModel model,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        var postsQuery = mapper.Map<PostQuery>(model);

        var postList = await blogRepository.GetPagedPostsAsync(
            postsQuery,
            model,
            p => p.ProjectToType<PostDto>());

        var paginationResult = new PaginationResult<PostDto>(postList);
        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> GetFeaturedPost(
        int limit,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        var postList = await blogRepository.GetPopularArticlesAsync(limit);
        var postDto = mapper.Map<IList<PostDto>>(postList);


        return Results.Ok(postDto);
    }

    private static async Task<IResult> GetRandomPost(
        int limit,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        var postList = await blogRepository.GetRandomPostAsync(limit);
        var postDto = mapper.Map<IList<PostDto>>(postList);


        return Results.Ok(postDto);
    }

    private static async Task<IResult> GetArchives(
        int month,
        IBlogRepository blogRepository)
    {
	    var result = await blogRepository.CountPostByMonth(month);
        return Results.Ok(result);
    }

    private static async Task<IResult> GetPostById(
	    Guid id,
	    IBlogRepository blogRepository,
	    IMapper mapper)
    {
	    var post = await blogRepository.GetPostByIdAsync(id, true);

        var postDetail = mapper.Map<PostDetail>(post);
		return postDetail != null
			? Results.Ok(postDetail)
		    : Results.NotFound($"Không tìm thấy bài viết với id: `{id}`");
    }

    private static async Task<IResult> GetPostBySlug(
		[FromRoute] string slug,
	    IBlogRepository blogRepository,
	    IMapper mapper)
    {
	    var post = await blogRepository.GetPostAsync(0, 0, 0, slug);

	    var postDetail = mapper.Map<PostDetail>(post);
	    return postDetail != null
		    ? Results.Ok(postDetail)
		    : Results.NotFound($"Không tìm thấy bài viết với mã định danh: `{slug}`");
    }

	private static async Task<IResult> AddPost(
        PostEditModel model,
        IBlogRepository blogRepository,
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        if (await blogRepository.IsPostSlugExistedAsync(Guid.Empty, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug {model.UrlSlug} đã được sử dụng");
        }

        var isExitsCategory = await blogRepository.GetCategoryByIdAsync(model.CategoryId);
        var isExitsAuthor = await authorRepository.GetAuthorByIdAsync(model.AuthorId);

        if (isExitsAuthor == null || isExitsCategory == null)
        {
			return Results.NotFound(
				$"Mã tác giả hoặc chủ đề không tồn tại!");
		}

        var post = mapper.Map<Post>(model);

        post.Id = Guid.NewGuid();
        post.PostedDate = DateTime.Now;
        
		await blogRepository.AddOrUpdatePostAsync(post, model.SelectedTags);

        return Results.CreatedAtRoute("GetPostById", new { post.Id },
            mapper.Map<PostDetail>(post));
    }

    private static async Task<IResult> UpdatePost(
        Guid id,
        PostEditModel model,
        IBlogRepository blogRepository,
        IAuthorRepository authorRepository,
		IMapper mapper)
    {
	    var post = await blogRepository.GetPostByIdAsync(id);

	    if (post == null)
	    {
			return Results.NotFound(
				$"Không tìm thấy bài viết với id: `{id}`");
		}

		if (await blogRepository.IsPostSlugExistedAsync(id, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug {model.UrlSlug} đã được sử dụng");
        }

        var isExitsCategory = await blogRepository.GetCategoryByIdAsync(model.CategoryId);
        var isExitsAuthor = await authorRepository.GetAuthorByIdAsync(model.AuthorId);

        if (isExitsAuthor == null || isExitsCategory == null)
        {
	        return Results.NotFound(
		        $"Mã tác giả hoặc chủ đề không tồn tại!");
        }

        mapper.Map(model, post);
        post.Id = id;
        post.Category = null;
        post.Author = null;

        return await blogRepository.AddOrUpdatePostAsync(post, model.SelectedTags) != null
            ? Results.NoContent()
            : Results.NotFound();
    }

    private static async Task<IResult> TogglePublicStatus(
	    Guid id,
	    IBlogRepository blogRepository)
    {
	    var oldPost = await blogRepository.GetPostByIdAsync(id);

	    if (oldPost == null)
	    {
		    return Results.NotFound($"Không tìm thấy bài viết với id: `{id}`");
	    }
	    await blogRepository.TogglePublicStatusPostAsync(id);
	    return Results.Ok();
    }


	private static async Task<IResult> SetPostPicture(
	    Guid id, IFormFile imageFile,
	    IBlogRepository blogRepository,
	    IMediaManager mediaManager)
    {
	    var oldPost = await blogRepository.GetPostByIdAsync(id);
	    if (oldPost == null)
	    {
		    return Results.NotFound(
			    $"Không tìm thấy bài viết với id: `{id}`");
	    }

		await mediaManager.DeleteFileAsync(oldPost.ImageUrl);

		var imageUrl = await mediaManager.SaveFileAsync(
		    imageFile.OpenReadStream(),
		    imageFile.FileName, imageFile.ContentType);

	    if (string.IsNullOrWhiteSpace(imageUrl))
	    {
		    return Results.BadRequest("Không lưu được tệp");
	    }

	    await blogRepository.SetImageUrlAsync(id, imageUrl);
	    return Results.Ok(imageUrl);
    }

	private static async Task<IResult> DeletePost(
        Guid id,
        IBlogRepository blogRepository,
        IMediaManager _media)
    {
	    var oldPost = await blogRepository.GetPostByIdAsync(id);

	    await _media.DeleteFileAsync(oldPost.ImageUrl);

		return await blogRepository.DeletePostByIdAsync(id)
            ? Results.NoContent()
            : Results.NotFound($"Không tìm thấy bài viết với id: `{id}`");

    }
}