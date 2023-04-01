namespace TatBlog.WebApp.Areas.Admin.Models;

public class TagEditModel
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? UrlSlug { get; set; }

    public string? Description { get; set; }

}