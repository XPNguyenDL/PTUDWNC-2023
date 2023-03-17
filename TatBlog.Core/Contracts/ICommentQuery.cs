using System.ComponentModel;
using TatBlog.Core.Entities;

namespace TatBlog.Core.Contracts;

public interface ICommentQuery
{
    [DisplayName("Từ khóa")]
    public string Keyword { get; set; }

    [DisplayName("Năm")]
    public int? Year { get; set; }

    [DisplayName("Tháng")]
    public int? Month { get; set; }
    
    public bool Active { get; set; }
    public CommentStatus CommentStatus { get; set; }
}