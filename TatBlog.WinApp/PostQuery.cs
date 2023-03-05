using TatBlog.Core.Contracts;

namespace TatBlog.WinApp;

public class PostQuery : IPostQuery
{
    public Guid AuthorId { get; set; } = Guid.NewGuid();
    public Guid CategoryId { get; set; } = Guid.NewGuid();
    public string CategorySlug { get; set; } = "";
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public string TagName { get; set; } = "";
}