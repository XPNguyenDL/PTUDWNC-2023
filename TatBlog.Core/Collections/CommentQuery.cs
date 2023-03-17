using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Core.Collections;

public class CommentQuery : ICommentQuery
{
    public string Keyword { get; set; } = "";
    public int? Year { get; set; } = 0;
    public int? Month { get; set; } = 0;
    public bool Active { get; set; } = false;
    public CommentStatus CommentStatus { get; set; } = CommentStatus.None;
}