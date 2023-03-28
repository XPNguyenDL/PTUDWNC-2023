using System.ComponentModel.DataAnnotations;
using TatBlog.WebApi.Extensions;

namespace TatBlog.WebApi.Endpoints;

public static class ContactEndpoints
{
	public static WebApplication MapContactEndpoints(
		this WebApplication app)
	{
		var routeGroupBuilder = app.MapGroup("/api/contact");

		routeGroupBuilder.MapGet("/", Contact)
			.WithName("Contact");

		return app;
	}

	private static IResult Contact(
		[Display(Name = "Email")] string email,
		[Display(Name = "Chủ đề")] string title,
		[Display(Name = "Nội dung")]  string content)
	{
		content = content.Replace("\n", "<br>");
		var subject = $"Phản hồi từ {email}";
		var body = $"{title}:<br> {content}";

		SendMailExtensions.SendEmail("2014478@dlu.edu.vn", subject, body);
		return Results.Ok("Gửi thành công");
	}
}