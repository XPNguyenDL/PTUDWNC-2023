using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApi.Models.PostModel;

public class PostEditModel
{
    public string Title { get; set; }

    public string ShortDescription { get; set; }

    public string Description { get; set; }

    // Meta data
    public string Meta { get; set; }

    public string UrlSlug { get; set; }

    public Guid CategoryId { get; set; }

    public Guid AuthorId { get; set; }

    public bool Published { get; set; }

	public IList<string> SelectedTags { get; set; }

    public PostEditModel()
    {
        SelectedTags = new List<string>();
    }
}