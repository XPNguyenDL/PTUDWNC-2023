namespace TatBlog.Core.Contracts;

public interface IPostQuery
{
    public Guid AuthorId { get; set; }

    public Guid CategoryId { get; set; }

    public string CategorySlug { get; set; }

    public string AuthorSlug { get; set; }

    public DateTime CreatedDate { get; set; }

    public bool Published { get; set; }
    
    public string TagSlug { get; set; }

    public string TagName { get; set; }

    public string Keyword { get; set; }
}