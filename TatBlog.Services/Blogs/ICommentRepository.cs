using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface ICommentRepository
{
    Task<Comment> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default); 
    Task<bool> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default); 
    Task<Comment> VerifyCommentAsync(Guid id, CommentStatus status, CancellationToken cancellationToken = default); 

    
}