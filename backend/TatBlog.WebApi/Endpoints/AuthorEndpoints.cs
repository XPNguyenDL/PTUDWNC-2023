using System.Net;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Media;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.AuthorModel;
using TatBlog.WebApi.Models.PostModel;

namespace TatBlog.WebApi.Endpoints;

public static class AuthorEndpoints
{
	public static WebApplication MapAuthorEndpoints(
		this WebApplication app)
	{

		var routeGroupBuilder = app.MapGroup("/api/authors");

		routeGroupBuilder.MapGet("/", GetAuthors)
			.WithName("GetAuthors")
			.Produces<ApiResponse<PaginationResult<AuthorItem>>>();

		routeGroupBuilder.MapGet("/{id:guid}", GetAuthorDetail)
			.WithName("GetAuthorById")
			.Produces<ApiResponse<AuthorItem>>()
			.Produces(404);

		routeGroupBuilder.MapGet("/best/{limit:int}", GetPopularAuthors)
			.WithName("GetPopularAuthors")
			.Produces<ApiResponse<AuthorItem>>();

		routeGroupBuilder.MapGet("/{id:guid}/posts", GetPostsByAuthorId)
			.WithName("GetPostsByAuthorId")
			.Produces<ApiResponse<PaginationResult<PostDto>>>();

		routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByAuthorSlug)
			.WithName("GetPostsByAuthorSlug")
			.Produces<ApiResponse<PaginationResult<PostDto>>>();

		routeGroupBuilder.MapPost("/", AddAuthor)
			.WithName("AddNewAuthor")
			.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
			.Produces(201)
			.Produces(400)
			.Produces(409);

		routeGroupBuilder.MapPost("/{id:guid}/avatar", SetAuthorPicture)
			.WithName("SetAuthorPicture")
			.Accepts<IFormFile>("multipart/form-data")
			.Produces<ApiResponse<string>>()
			.Produces(400);

		routeGroupBuilder.MapPut("/{id:guid}", UpdateAuthor)
			.WithName("UpdateAuthor")
			.AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
			.Produces(204)
			.Produces(400)
			.Produces(409);

		routeGroupBuilder.MapDelete("/{id:guid}", DeleteAuthor)
			.WithName("DeleteAuthor")
			.Produces(204)
			.Produces(404);

		return app;
	}

	private static async Task<IResult> GetAuthors(
		[AsParameters] AuthorFilterModel model,
		IAuthorRepository authorRepository)
	{
		var authorList = await authorRepository.GetPagedAuthorsAsync(model, model.Name);

		var paginationResult = new PaginationResult<AuthorItem>(authorList);

		return Results.Ok(ApiResponse.Success(paginationResult));
	}

	private static async Task<IResult> GetAuthorDetail(
		Guid id,
		IAuthorRepository authorRepository,
		IMapper mapper)
	{
		var author = await authorRepository.GetCachedAuthorByIdAsync(id);

		return author == null
			? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy tác giả có Id: {id}"))
			: Results.Ok(ApiResponse.Success(mapper.Map<AuthorItem>(author)));
	}

	private static async Task<IResult> GetPostsByAuthorId(
		Guid id,
		[AsParameters] PagingModel pagingModel,
		IBlogRepository blogRepository)
	{
		var postQuery = new PostQuery()
		{
			AuthorId = id,
			Published = true
		};

		var postList = await blogRepository.GetPagedPostsAsync(
			postQuery, pagingModel,
			post => post.ProjectToType<PostDto>());

		var paginationResult = new PaginationResult<PostDto>(postList);

		return Results.Ok(ApiResponse.Success(paginationResult));
	}

	private static async Task<IResult> GetPostsByAuthorSlug(
		[FromRoute] string slug,
		[AsParameters] PagingModel pagingModel,
		IBlogRepository blogRepository)
	{
		var postsQuery = new PostQuery()
		{
			AuthorSlug = slug,
			Published = true
		};

		var postList = await blogRepository.GetPagedPostsAsync(
			postsQuery,
			pagingModel,
			p => p.ProjectToType<PostDto>());

		var paginationResult = new PaginationResult<PostDto>(postList);
		return Results.Ok(ApiResponse.Success(paginationResult));
	}

	private static async Task<IResult> AddAuthor(
		AuthorEditModel model,
		IAuthorRepository authorRepository,
		IMapper mapper)
	{
		if (await authorRepository.IsExistAuthorSlugAsync(Guid.Empty, model.UrlSlug))
		{
			return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict,
				$"Slug {model.UrlSlug} đã được sử dụng"));
		}

		var author = mapper.Map<Author>(model);

		await authorRepository.AddOrUpdateAuthorAsync(author);

		return Results.Ok(ApiResponse.Success(
			mapper.Map<AuthorItem>(author), HttpStatusCode.Created));
	}

	private static async Task<IResult> SetAuthorPicture(
		Guid id, IFormFile imageFile,
		IAuthorRepository authorRepository,
		IMediaManager mediaManager)
	{
		var imageUrl = await mediaManager.SaveFileAsync(
			imageFile.OpenReadStream(),
			imageFile.FileName, imageFile.ContentType);

		if (string.IsNullOrWhiteSpace(imageUrl))
		{
			return Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Không lưu được tệp"));
		}

		await authorRepository.SetImageUrlAsync(id, imageUrl);
		return Results.Ok(ApiResponse.Success(imageUrl));
	}

	private static async Task<IResult> UpdateAuthor(
		Guid id,
		AuthorEditModel model,
		IAuthorRepository authorRepository,
		IMapper mapper)
	{
		if (await authorRepository.IsExistAuthorSlugAsync(id, model.UrlSlug))
		{
			return Results.Ok(ApiResponse.Fail(HttpStatusCode.Conflict, $"Slug {model.UrlSlug} đã được sử dụng"));
		}

		var author = mapper.Map<Author>(model);
		author.Id = id;

		return await authorRepository.AddOrUpdateAuthorAsync(author) != null
			? Results.Ok(ApiResponse.Success("Author is updated", HttpStatusCode.NoContent))
			: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound));
	}

	private static async Task<IResult> DeleteAuthor(
		Guid id,
		IAuthorRepository authorRepository)
	{

		return await authorRepository.DeleteAuthorByIdAsync(id)
			? Results.Ok(ApiResponse.Success("Author id deleted", HttpStatusCode.NoContent))
			: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy author với id: `{id}`"));
	}

	private static async Task<IResult> GetPopularAuthors(
		int limit,
		IAuthorRepository authorRepository)
	{
		var author = await authorRepository.GetAuthorMostPost(limit);

		return Results.Ok(ApiResponse.Success(author));
	}
}