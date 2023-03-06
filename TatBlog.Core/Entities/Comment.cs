using System.ComponentModel.DataAnnotations.Schema;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities;

public enum CommentStatus
{
    Violate, // Vi phạm nội quy
    Valid // hợp lệ
}
public class Comment : IEntity
{
    public Guid Id { get; set; }
    public string UserComment { get; set; }
    public string Content { get; set; }
    public DateTime PostTime { get; set; }
    public bool Active { get; set; }

    public CommentStatus CommentStatus { get; set; }
    
    
    [ForeignKey("Post")]
    public Guid PostId { get; set; } // Mã bài viết
    public virtual Post Post { get; set; }
}