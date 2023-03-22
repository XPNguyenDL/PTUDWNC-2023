using Mapster;
using MapsterMapper;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Services.Blogs;
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

        routeGroupBuilder.MapGet("/{id:guid}/posts", GetPostsByAuthorId)
            .WithName("GetPostsByAuthorId")
            .Produces<PaginationResult<PostDto>>();

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
}