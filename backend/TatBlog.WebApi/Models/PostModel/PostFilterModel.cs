using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApi.Models.PostModel;

public class PostFilterModel : PagingModel
{
    public string Keyword { get; set; }
    public string CategorySlug { get; set; }
    public string AuthorSlug { get; set; }
    public string PostSlug { get; set; }
}