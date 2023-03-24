using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.Globalization;

namespace TatBlog.WebApi.Models;

public class PostFilterModel : PagingModel
{
    public string? Keyword { get; set; }
}