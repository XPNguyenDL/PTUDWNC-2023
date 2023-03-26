using TatBlog.WebApi.Models.Author;
using TatBlog.WebApi.Models.CategoryModel;
using TatBlog.WebApi.Models.TagModel;

namespace TatBlog.WebApi.Models.PostModel;

public class PostDto
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string UrlSlug { get; set; }

    public string ImageUrl { get; set; }

    public int ViewCount { get; set; }

    public DateTime PostDate { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public SubscriberDto Category { get; set; }

    public AuthorDto Author { get; set; }

    public IList<TagDto> Tags { get; set; }
}