namespace TatBlog.Core.DTO;

public class TagItem
{
    public Guid Id { get; set; }    

    public string Name { get; set; }

    public string Description { get; set; }

    public string UrlSlug { get; set; }
    public int PostCount { get; set; }
}