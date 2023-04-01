using System.Net;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Filters;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.CommentModel;
using TatBlog.WebApi.Models.PostModel;
using TatBlog.WebApi.Models.TagModel;

namespace TatBlog.WebApi.Endpoints;

public static class CommentEndpoints
{
    public static WebApplication MapCommentEndpoints(
        this WebApplication app)
    {
        var routeGroupBuilder = app.MapGroup("/api/comments");

        routeGroupBuilder.MapGet("/", GetComment)
            .WithName("GetComment")
            .Produces<ApiResponse<PaginationResult<Comment>>>();

        routeGroupBuilder.MapGet("/{id:guid}/posts", GetCommentByPostId)
	        .WithName("GetCommentByPostId")
	        .Produces<ApiResponse<IList<Comment>>>();

		routeGroupBuilder.MapGet("/{id:guid}", GetCommentById)
            .WithName("GetCommentById")
            .Produces<ApiResponse<Comment>>();

        routeGroupBuilder.MapPost("/", AddComment)
            .WithName("AddComment")
            .Produces(201)
            .Produces(400)
            .Produces(409);

        routeGroupBuilder.MapGet("/verify/{id:guid}", VerifyComment)
	        .WithName("VerifyComment")
	        .Produces(204)
	        .Produces(404);


		routeGroupBuilder.MapDelete("/{id:guid}", DeleteComment)
            .WithName("DeleteComment")
            .Produces(204)
            .Produces(404);

        return app;
    }

    private static async Task<IResult> GetComment(
        [AsParameters] CommentFilterModel model,
        ICommentRepository cmtRepository)
    {
		var cmtList = await cmtRepository.GetPagedCommentAsync(model.Keyword, model);
        
		var paginationResult = new PaginationResult<Comment>(cmtList);

        return Results.Ok(ApiResponse.Success(paginationResult));
    }

    private static async Task<IResult> GetCommentById(
	    Guid id,
	    ICommentRepository cmtRepository)
    {
		var cmt = await cmtRepository.GetCommentsById(id);

		return cmt == null
			? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound))
			: Results.Ok(ApiResponse.Success(cmt));
	}

	private static async Task<IResult> GetCommentByPostId(
        Guid id,
        ICommentRepository cmtRepository,
        IMapper mapper)
    {
        var cmtList = await cmtRepository.GetCommentsByPost(id);

        return cmtList.Count == 0
            ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Không có bình luận nào"))
            : Results.Ok(ApiResponse.Success(cmtList));
    }

    private static async Task<IResult> VerifyComment(
        Guid id,
        CommentStatus status,
        ICommentRepository cmtRepository)
    {
        var cmt = await cmtRepository.VerifyCommentAsync(id, status);

        return cmt == null
	        ? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound))
	        : Results.Ok(ApiResponse.Success(cmt));
    }

    private static async Task<IResult> AddComment(
	    CommentEditModel model,
        ICommentRepository cmtRepository,
        IMapper mapper)
    {

	    var newCmt = new Comment()
	    {
		    Id = Guid.NewGuid(),
		    PostId = model.PostId,
		    Active = false,
		    CommentStatus = CommentStatus.NotVerify,
		    Content = model.Content,
		    UserComment = model.Content,
		    PostTime = DateTime.Now
	    };

		await cmtRepository.AddOrUpdateCommentAsync(newCmt);

        return Results.Ok(
	        ApiResponse.Success(mapper.Map<CommentDto>(newCmt), HttpStatusCode.Created));
    }

    private static async Task<IResult> DeleteComment(
        Guid id,
        ICommentRepository cmtRepository)
    {
        return await cmtRepository.DeleteCommentAsync(id)
            ? Results.Ok(ApiResponse.Success("Comment is deleted", HttpStatusCode.NoContent))
            : Results.Ok(ApiResponse.Fail(
	            HttpStatusCode.NotFound, $"Không tìm thấy bình luận với id: `{id}`"));
    }
}