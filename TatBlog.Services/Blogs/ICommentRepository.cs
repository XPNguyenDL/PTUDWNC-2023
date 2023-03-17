using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface ICommentRepository
{

    Task<List<Comment>> GetCommentsByPost(Guid postId, CancellationToken cancellationToken = default);
    Task<IPagedList<Comment>> GetPagedCommentAsync(ICommentQuery condition, IPagingParams pagingParams,
        CancellationToken cancellationToken = default);
    Task<bool> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default); 
    Task<bool> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default); 
    Task<int> CountCommentAsync(CancellationToken cancellationToken = default); 
    Task<int> CountCommentNotVerifyAsync(CancellationToken cancellationToken = default); 
    Task<Comment> VerifyCommentAsync(Guid id, CommentStatus status, CancellationToken cancellationToken = default);

}