namespace TatBlog.WebApi.Models.TagModel;

public class TagDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string UrlSlug { get; set; }

    public string Description { get; set; }
}