using TatBlog.Core.Contracts;

namespace TatBlog.Core.Collections;

public class CategoryQuery : ICategoryQuery
{
    public string Keyword { get; set; } = "";
    public bool ShowOnMenu { get; set; } = false;
}