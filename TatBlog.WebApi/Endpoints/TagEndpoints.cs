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
using TatBlog.WebApi.Models.PostModel;
using TatBlog.WebApi.Models.TagModel;

namespace TatBlog.WebApi.Endpoints;

public static class TagEndpoints
{
    public static WebApplication MapTagEndpoints(
        this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/tags");

        routeGroupBuilder.MapGet("/", GetTag)
            .WithName("GetTag")
            .Produces<PaginationResult<TagItem>>();

        routeGroupBuilder.MapGet("/{id:guid}", GetTagById)
            .WithName("GetTagById")
            .Produces<TagDto>();

        routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByTagSlug)
            .WithName("GetPostsByTagSlug")
            .Produces<PaginationResult<PostDto>>();

        routeGroupBuilder.MapPost("/", AddTag)
            .WithName("AddTag")
            .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
            .Produces(201)
            .Produces(400)
            .Produces(409);

        routeGroupBuilder.MapPut("/{id:guid}", UpdateTag)
            .WithName("UpdateTag")
            .AddEndpointFilter<ValidatorFilter<TagEditModel>>()
            .Produces(204)
            .Produces(400)
            .Produces(409);

        routeGroupBuilder.MapDelete("/{id:guid}", DeleteTag)
            .WithName("DeleteTag")
            .Produces(204)
            .Produces(404);

        return app;
    }

    private static async Task<IResult> GetTag(
        [AsParameters] TagFilterModel model,
        IBlogRepository blogRepository)
    {
        var tagList = await blogRepository.GetPagedTagsAsync(model.Name, model);
        var paginationResult = new PaginationResult<TagItem>(tagList);

        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> GetTagById(
        Guid id,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        var tag = await blogRepository.GetTagByIdAsync(id);
        var tagItem = mapper.Map<TagDto>(tag);

        return tag == null
            ? Results.NotFound(404)
            : Results.Ok(tagItem);
    }

    private static async Task<IResult> GetPostsByTagSlug(
        [FromRoute] string slug,
        [AsParameters] PagingModel pagingModel,
        IBlogRepository blogRepository)
    {
        var postsQuery = new PostQuery()
        {
            TagSlug = slug,
            Published = true
        };

        var postList = await blogRepository.GetPagedPostsAsync(
            postsQuery,
            pagingModel,
            p => p.ProjectToType<PostDto>());

        var paginationResult = new PaginationResult<PostDto>(postList);
        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> AddTag(
        TagEditModel model,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        if (await blogRepository.IsTagSlugExistedAsync(Guid.Empty, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug {model.UrlSlug} đã được sử dụng");
        }

        var tag = mapper.Map<Tag>(model);

        await blogRepository.AddOrUpdateTagAsync(tag);

        return Results.CreatedAtRoute("GetTagById", new { tag.Id },
            mapper.Map<TagItem>(tag));
    }

    private static async Task<IResult> UpdateTag(
        Guid id,
        TagEditModel model,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        if (await blogRepository.IsTagSlugExistedAsync(id, model.UrlSlug))
        {
            return Results.Conflict(
                $"Slug {model.UrlSlug} đã được sử dụng");
        }

        var tag = mapper.Map<Tag>(model);
        tag.Id = id;

        return await blogRepository.AddOrUpdateTagAsync(tag) != null
            ? Results.NoContent()
            : Results.NotFound();
    }

    private static async Task<IResult> DeleteTag(
        Guid id,
        IBlogRepository blogRepository)
    {
        return await blogRepository.DeleteTagByIdAsync(id)
            ? Results.NoContent()
            : Results.NotFound($"Không tìm thấy thẻ với id: `{id}`");
    }
}