using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;

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
			.Produces(404); ;

        //routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
        //    .WithName("GetPostsByCategorySlug")
        //    .Produces<PaginationResult<PostDto>>();

        //routeGroupBuilder.MapPost("/", AddCategory)
        //    .WithName("AddCategory")
        //    .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
        //    .Produces(201)
        //    .Produces(400)
        //    .Produces(409);

        //routeGroupBuilder.MapPut("/{id:guid}", UpdateCategory)
        //    .WithName("UpdateCategory")
        //    .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
        //    .Produces(204)
        //    .Produces(400)
        //    .Produces(409);

        //routeGroupBuilder.MapDelete("/{id:guid}", DeleteCategory)
        //    .WithName("DeleteCategory")
        //    .Produces(204)
        //    .Produces(404);

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

	private static async Task<IResult> AddCategory(
        CategoryEditModel model,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        if (await blogRepository.IsCategorySlugExistedAsync(Guid.Empty, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug {model.UrlSlug} đã được sử dụng");
        }

        var category = mapper.Map<Category>(model);

        await blogRepository.AddOrUpdateCategoryAsync(category);

        return Results.CreatedAtRoute("GetCategoryById", new { category.Id },
            mapper.Map<AuthorItem>(category));
    }

    private static async Task<IResult> UpdateCategory(
        Guid id,
        CategoryEditModel model,
        IValidator<CategoryEditModel> validator,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        if (await blogRepository.IsCategorySlugExistedAsync(id, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug {model.UrlSlug} đã được sử dụng");
        }

        var category = mapper.Map<Category>(model);
        category.Id = id;

        return await blogRepository.AddOrUpdateCategoryAsync(category) != null
            ? Results.NoContent()
            : Results.NotFound();
    }

    private static async Task<IResult> DeleteCategory(
        Guid id,
        IBlogRepository blogRepository)
    {
        return await blogRepository.DeleteCategoryByIdAsync(id)
            ? Results.NoContent()
            : Results.NotFound($"Không tìm thấy chủ đề với id: `{id}`");
    }
}