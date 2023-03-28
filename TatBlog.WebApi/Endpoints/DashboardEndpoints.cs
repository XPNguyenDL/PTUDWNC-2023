using TatBlog.Services.Blogs;
using TatBlog.WebApi.Models.DashboardModel;

namespace TatBlog.WebApi.Endpoints;

public static class DashboardEndpoints
{
	public static WebApplication MapDashboardEndpoints(
		this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/dashboards");

		routeGroupBuilder.MapGet("/", GetDashboardInformation)
			.WithName("GetDashboardInformation")
			.Produces<DashboardModel>();

		return app;
	}


	private static async Task<IResult> GetDashboardInformation(
		IBlogRepository blogRepo,
		ICommentRepository cmtRepo,
		IAuthorRepository authorRepo,
		ISubscriberRepository subRepo)
	{
		var result = new DashboardModel()
		{
			PostCount = await blogRepo.CountPostAsync(),
			PostUnPublicCount = await blogRepo.CountPostUnPublicAsync(),
			AuthorCount = await authorRepo.CountAuthorAsync(),
			CmtCount = await cmtRepo.CountCommentAsync(),
			CmtNotVerifyCount = await cmtRepo.CountCommentNotVerifyAsync(),
			SubCount = await subRepo.CountSubscriberAsync(),
			SubDailyCount = await subRepo.CountSubscriberByDayAsync(),
		};

		return Results.Ok(result);
	}
}