using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Media;
using TatBlog.WebApi.Models;

namespace TatBlog.WebApi.Endpoints;

public static class AuthorEndpoints
{
    public static WebApplication MapAuthorEndpoints(
        this WebApplication app)
    {

        var routeGroupBuilder = app.MapGroup("/api/authors");

        routeGroupBuilder.MapGet("/", GetAuthors)
            .WithName("GetAuthors")
            .Produces<PaginationResult<AuthorItem>>();

        routeGroupBuilder.MapGet("/{id:guid}", GetAuthorDetail)
            .WithName("GetAuthorById")
            .Produces<AuthorItem>()
            .Produces(404);

        routeGroupBuilder.MapGet("/best/{limit:int}", GetPopularAuthors)
            .WithName("GetPopularAuthors")
            .Produces<AuthorItem>();

        routeGroupBuilder.MapGet("/{id:guid}/posts", GetPostsByAuthorId)
            .WithName("GetPostsByAuthorId")
            .Produces<PaginationResult<PostDto>>();

        routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByAuthorSlug)
            .WithName("GetPostsByAuthorSlug")
            .Produces<PaginationResult<PostDto>>();

        routeGroupBuilder.MapPost("/", AddAuthor)
            .WithName("AddNewAuthor")
            .AddEndpointFilter<ValidatorFilter<AuthorEditModel>>()
            .Produces(201)
            .Produces(400)
            .Produces(409);

        routeGroupBuilder.MapPost("/{id:guid}/avatar", SetAuthorPicture)
            .WithName("SetAuthorPicture")
            .Accepts<IFormFile>("multipart/form-data")
            .Produces<string>()
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

        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> GetAuthorDetail(
        Guid id,
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        var author = await authorRepository.GetCachedAuthorByIdAsync(id);

        return author == null
            ? Results.NotFound($"Không tìm thấy tác giả có Id: {id}")
            : Results.Ok(mapper.Map<AuthorItem>(author));
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

        return Results.Ok(paginationResult);
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
        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> AddAuthor(
        AuthorEditModel model,
        IValidator<AuthorEditModel> validator,
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        if (await authorRepository.IsExistAuthorSlugAsync(Guid.Empty, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug {model.UrlSlug} đã được sử dụng");
        }

        var author = mapper.Map<Author>(model);

        await authorRepository.AddOrUpdateAuthorAsync(author);

        return Results.CreatedAtRoute("GetAuthorById", new { author.Id },
            mapper.Map<AuthorItem>(author));
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
            return Results.BadRequest("Không lưu được tệp");
        }

        await authorRepository.SetImageUrlAsync(id, imageUrl);
        return Results.Ok(imageUrl);
    }

    private static async Task<IResult> UpdateAuthor(
        Guid id, 
        AuthorEditModel model,
        IValidator<AuthorEditModel> validator,
        IAuthorRepository authorRepository,
        IMapper mapper)
    {
        if (await authorRepository.IsExistAuthorSlugAsync(id, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug {model.UrlSlug} đã được sử dụng");
        }

        var author = mapper.Map<Author>(model);
        author.Id = id;

        return await authorRepository.AddOrUpdateAuthorAsync(author) != null
            ? Results.NoContent()
            : Results.NotFound();
    }

    private static async Task<IResult> DeleteAuthor(
        Guid id,
        IAuthorRepository authorRepository)
    {

        return await authorRepository.DeleteAuthorByIdAsync(id)
            ? Results.NoContent()
            : Results.NotFound($"Không tìm thấy author với id: `{id}`");
    }

    private static async Task<IResult> GetPopularAuthors(
        int limit,
        IAuthorRepository authorRepository)
    {
        var author = await authorRepository.GetAuthorMostPost(limit);

        return Results.Ok(author);
    }

}