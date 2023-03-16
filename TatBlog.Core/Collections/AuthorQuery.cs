using TatBlog.Core.Contracts;

namespace TatBlog.Core.Collections;

public class AuthorQuery : IAuthorQuery
{
    public string Keyword { get; set; } = "";
    public int Month { get; set; } = 0;
    public int Year { get; set; } = 0;
}