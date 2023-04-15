using TatBlog.Core.Entities;

namespace TatBlog.WebApi.Models.CommentModel;

public class CommentEditModel
{
	public Guid PostId { get; set; }

	public string UserComment { get; set; } = "";

	public string Content { get; set; } = "";
	
}