namespace TatBlog.WebApi.Models;

public class TagDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string UrlSlug { get; set; }
}