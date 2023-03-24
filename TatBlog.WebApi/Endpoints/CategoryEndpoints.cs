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

public static class CategoryEndpoints
{
    public static WebApplication MapCategoryEndpoints(
        this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/categories");

        routeGroupBuilder.MapGet("/", GetCategories)
            .WithName("GetCategories")
            .Produces<PaginationResult<CategoryItem>>();

        routeGroupBuilder.MapGet("/{id:guid}", GetCategoryById)
            .WithName("GetCategoryById")
            .Produces<Category>();

        routeGroupBuilder.MapGet("/{slug:regex(^[a-z0-9_-]+$)}/posts", GetPostsByCategorySlug)
            .WithName("GetPostsByCategorySlug")
            .Produces<PaginationResult<PostDto>>();

        routeGroupBuilder.MapPost("/", AddCategory)
            .WithName("AddCategory")
            .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
            .Produces(201)
            .Produces(400)
            .Produces(409);

        routeGroupBuilder.MapPut("/{id:guid}", UpdateCategory)
            .WithName("UpdateCategory")
            .AddEndpointFilter<ValidatorFilter<CategoryEditModel>>()
            .Produces(204)
            .Produces(400)
            .Produces(409);

        routeGroupBuilder.MapDelete("/{id:guid}", DeleteCategory)
            .WithName("DeleteCategory")
            .Produces(204)
            .Produces(404);

        return app;
    }

    private static async Task<IResult> GetCategories(
        [AsParameters] CategoryFilterModel model,
        IBlogRepository blogRepository)
    {
        var categoryList = await blogRepository.GetPagedCategoriesAsync(model, model.Name);

        var paginationResult = new PaginationResult<CategoryItem>(categoryList);

        return Results.Ok(paginationResult);
    }

    private static async Task<IResult> GetCategoryById(
        Guid id,
        IBlogRepository blogRepository,
        IMapper mapper)
    {
        var category = await blogRepository.GetCategoryByIdAsync(id);
        var categoryItem = mapper.Map<CategoryItem>(category);


        return category == null
            ? Results.NotFound(404)
            : Results.Ok(categoryItem);
    }

    private static async Task<IResult> GetPostsByCategorySlug(
        [FromRoute] string slug,
        [AsParameters] PagingModel pagingModel,
        IBlogRepository blogRepository)
    {
        var postsQuery = new PostQuery()
        {
            CategorySlug = slug,
            Published = true
        };

        var postList = await blogRepository.GetPagedPostsAsync(
            postsQuery,
            pagingModel,
            p => p.ProjectToType<PostDto>());

        var paginationResult = new PaginationResult<PostDto>(postList);
        return Results.Ok(paginationResult);
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