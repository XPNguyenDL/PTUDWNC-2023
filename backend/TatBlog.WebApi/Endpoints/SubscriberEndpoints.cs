using System.Net;
using FluentEmail.Core;
using FluentEmail.Smtp;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApi.Extensions;
using TatBlog.WebApi.Models;
using TatBlog.WebApi.Models.PostModel;
using TatBlog.WebApi.Models.SubscriberModel;

namespace TatBlog.WebApi.Endpoints;

public static class SubscriberEndpoints
{
	public static WebApplication MapSubscriberEndpoints(
		this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/subscribers");

		routeGroupBuilder.MapGet("/", GetSubscriber)
			.WithName("GetSubscriber")
			.Produces<ApiResponse<PaginationResult<Subscriber>>>();

		routeGroupBuilder.MapGet("/{id:guid}", GetSubscriberById)
			.WithName("GetSubscriberById")
			.Produces(404);

		routeGroupBuilder.MapPost("/{email:regex(^[-\\w\\.]+@[-\\w]+(\\.[-\\w]+)*$)}/subscribe", Subscribe)
			.WithName("Subscribe")
			.Produces(204)
			.Produces(400);

		routeGroupBuilder.MapPost("/{email:regex(^[-\\w\\.]+@[-\\w]+(\\.[-\\w]+)*$)}/unsubscribe", UnSubscribe)
			.WithName("UnSubscribe")
			.Produces(204)
			.Produces(400);

		routeGroupBuilder.MapPost("/block/", BlockSubscriber)
			.WithName("BlockSubscriber")
			.Produces(201)
			.Produces(400)
			.Produces(409);

		routeGroupBuilder.MapDelete("/{id:guid}", DeleteSubscriber)
			.WithName("DeleteSubscriber")
			.Produces(204)
			.Produces(404);

		return app;
	}

	private static async Task<IResult> GetSubscriber(
		[AsParameters] SubscriberFilterModel model,
		ISubscriberRepository subscriberRepository,
		IMapper mapper)
	{
		var subQuery = mapper.Map<SubscriberQuery>(model);

		var subList = await subscriberRepository.SearchSubscribersAsync(subQuery, model);
		var paginationResult = new PaginationResult<Subscriber>(subList);

		return Results.Ok(ApiResponse.Success(paginationResult));
	}

	private static async Task<IResult> GetSubscriberById(
		Guid id,
		ISubscriberRepository subscriberRepository,
		IMapper mapper)
	{
		var sub = await subscriberRepository.GetSubscriberByIdAsync(id);
		var subDto = mapper.Map<SubscriberDto>(sub);

		return subDto == null
			? Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, "Sub is not found"))
			: Results.Ok(ApiResponse.Success(subDto));
	}

	private static async Task<IResult> Subscribe(
		string email,
		ISubscriberRepository subRepository,
		IWebHostEnvironment env)
	{
		var subSuccess = await subRepository.SubscribeAsync(email);

		if (subSuccess)
		{
			SendMailExtensions.Initialize(env);
			var body = SendMailExtensions.SetTemplateEmail("templates/emails/EmailSubscribe.html");
			SendMailExtensions.SendEmail(email, "Thông báo đăng ký", body);
		}

		return subSuccess
			? Results.Ok(ApiResponse.Success("Đăng ký thành công"))
			: Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Đăng ký thất bại"));
	}

	private static async Task<IResult> UnSubscribe(
		string email,
		string reason,
		ISubscriberRepository subRepository)
	{
		var subSuccess = await subRepository.UnSubscribeAsync(email, reason);
		return subSuccess
			? Results.Ok(ApiResponse.Success("Hủy đăng ký thành công"))
			: Results.Ok(ApiResponse.Fail(HttpStatusCode.BadRequest, "Hủy đăng ký thất bại"));
	}

	private static async Task<IResult> BlockSubscriber(
		Guid id,
		SubscriberEditModel model,
		ISubscriberRepository subRepository,
		IMapper mapper)
	{

		var sub = await subRepository.GetSubscriberByIdAsync(id);

		await subRepository.BlockSubscribeAsync(sub.Id, model.Reason, model.Note);

		return Results.Ok(ApiResponse.Success(sub, HttpStatusCode.Created));
	}

	private static async Task<IResult> DeleteSubscriber(
		Guid id,
		ISubscriberRepository subRepository)
	{
		return await subRepository.DeleteSubscriberAsync(id)
			? Results.Ok(ApiResponse.Success("Sub is deleted", HttpStatusCode.NoContent))
			: Results.Ok(ApiResponse.Fail(HttpStatusCode.NotFound, $"Không tìm thấy người đăng ký với id: `{id}`"));
	}
}