using TatBlog.Core.Entities;

namespace TatBlog.Core.DTO;

public class PostQuery
{
    public Guid AuthorId { get; set; }
    public Guid CategoryId { get; set; }

    public string CategorySlug { get; set; }

    public DateTime CreatedDate { get; set; }

    public String TagName { get; set; }

}