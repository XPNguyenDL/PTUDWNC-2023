using FluentEmail.Core;
using FluentEmail.Smtp;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
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
			.Produces<PaginationResult<Subscriber>>();

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

		return Results.Ok(paginationResult);
	}

	private static async Task<IResult> GetSubscriberById(
		Guid id,
		ISubscriberRepository subscriberRepository,
		IMapper mapper)
	{
		var sub = await subscriberRepository.GetSubscriberByIdAsync(id);
		var subDto = mapper.Map<SubscriberDto>(sub);

		return subDto == null
			? Results.NotFound(404)
			: Results.Ok(subDto);
	}

	private static async Task<IResult> Subscribe(
		string email,
		ISubscriberRepository subRepository)
	{
		var subSuccess = await subRepository.SubscribeAsync(email);
		
		return subSuccess
			? Results.Ok("Đăng ký thành công")
			: Results.Problem("Đăng ký thất bại");
	}

	private static async Task<IResult> UnSubscribe(
		string email,
		string reason,
		ISubscriberRepository subRepository)
	{
		var subSuccess = await subRepository.UnSubscribeAsync(email, reason);
		return subSuccess
			? Results.Ok("Hủy đăng ký thành công")
			: Results.Problem("Hủy đăng ký thất bại");
	}

	private static async Task<IResult> BlockSubscriber(
		Guid id,
		SubscriberEditModel model,
		ISubscriberRepository subRepository,
		IMapper mapper)
	{

		var sub = await subRepository.GetSubscriberByIdAsync(id);

		await subRepository.BlockSubscribeAsync(sub.Id, model.Reason, model.Note);

		return Results.CreatedAtRoute("GetSubscriberById", new { sub.Id }, sub);
	}

	private static async Task<IResult> DeleteSubscriber(
		Guid id,
		ISubscriberRepository subRepository)
	{
		return await subRepository.DeleteSubscriberAsync(id)
			? Results.NoContent()
			: Results.NotFound($"Không tìm thấy người đăng ký với id: `{id}`");
	}
}