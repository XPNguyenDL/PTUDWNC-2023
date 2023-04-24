using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface ICommentRepository
{
	Task<List<Comment>> GetCommentsByPost(Guid postId, bool isGetAll = false, CancellationToken cancellationToken = default);
	Task<Comment> GetCommentsById(Guid id, CancellationToken cancellationToken = default);

	Task<IPagedList<Comment>> GetPagedCommentAsync(ICommentQuery condition, IPagingParams pagingParams,
		CancellationToken cancellationToken = default);
	Task<IPagedList<Comment>> GetPagedCommentAsync(string keyword, IPagingParams pagingParams,
		CancellationToken cancellationToken = default);

	Task<IPagedList<T>> GetPagedCommentAsync<T>(
		CommentQuery condition,
		IPagingParams pagingParams,
		Func<IQueryable<Comment>, IQueryable<T>> mapper);
	Task<bool> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default);
	Task<bool> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default);
	Task<int> CountCommentAsync(CancellationToken cancellationToken = default);
	Task<int> CountCommentNotVerifyAsync(CancellationToken cancellationToken = default);
	Task<Comment> VerifyCommentAsync(Guid id, CommentStatus status, CancellationToken cancellationToken = default);

}