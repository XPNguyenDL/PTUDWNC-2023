using TatBlog.Core.Entities;

namespace TatBlog.WebApi.Models.CommentModel;

public class CommentDto
{
	public Guid Id { get; set; }

	public string UserComment { get; set; }

	public string Content { get; set; }

	public DateTime PostTime { get; set; }

	public CommentStatus CommentStatus { get; set; }
}