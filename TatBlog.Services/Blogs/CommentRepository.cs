using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class CommentRepository : ICommentRepository
{
	private readonly BlogDbContext _dbContext;

	public CommentRepository(BlogDbContext context)
	{
		_dbContext = context;
	}

	public async Task<List<Comment>> GetCommentsByPost(Guid postId, bool isGetAll = false, CancellationToken cancellationToken = default)
	{
		var comment = _dbContext.Set<Comment>()
			.Include(s => s.Post);

		if (isGetAll)
		{
			return await comment.Where(s => s.PostId == postId).ToListAsync(cancellationToken);

		}
		return await comment.Where(s => s.PostId == postId && s.CommentStatus == CommentStatus.Valid).ToListAsync(cancellationToken);
	}


	public async Task<IPagedList<Comment>> GetPagedCommentAsync(ICommentQuery condition, IPagingParams pagingParams,
		CancellationToken cancellationToken = default)
	{
		var commentQuery = _dbContext.Set<Comment>()
			.Include(s => s.Post)
			.WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), s =>
				s.Content.Contains(condition.Keyword) ||
				s.UserComment.Contains(condition.Keyword) ||
				s.Post.Title.Contains(condition.Keyword))
			.WhereIf(condition.CommentStatus != CommentStatus.None, s =>
				s.CommentStatus == condition.CommentStatus)
			.WhereIf(condition.Year > 0, s => s.PostTime.Year == condition.Year)
			.WhereIf(condition.Month > 0, s => s.PostTime.Month == condition.Month);

		return await commentQuery.ToPagedListAsync(pagingParams, cancellationToken);
	}



	public async Task<bool> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
	{
		try
		{
			if (_dbContext.Set<Comment>().Any(s => s.Id == comment.Id))
			{
				_dbContext.Entry(comment).State = EntityState.Modified;
			}
			else
			{
				comment.CommentStatus = CommentStatus.None;
				_dbContext.Comments.Add(comment);
			}

			await _dbContext.SaveChangesAsync(cancellationToken);
			return true;
		}
		catch
		{
			return false;
		}
	}
	public async Task<bool> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await _dbContext.Set<Comment>()
			.Where(x => x.Id == id)
			.ExecuteDeleteAsync(cancellationToken) > 0;
	}

	public async Task<int> CountCommentAsync(CancellationToken cancellationToken = default)
	{
		return await _dbContext.Set<Comment>().CountAsync(cancellationToken);
	}

	public async Task<int> CountCommentNotVerifyAsync(CancellationToken cancellationToken = default)
	{
		return await _dbContext.Set<Comment>().CountAsync(s => s.CommentStatus == CommentStatus.NotVerify, cancellationToken);
	}

	public async Task<Comment> VerifyCommentAsync(Guid id, CommentStatus status, CancellationToken cancellationToken = default)
	{
		var cmt = _dbContext.Set<Comment>().FirstOrDefault(s => s.Id == id);
		if (cmt != null)
		{
			cmt.CommentStatus = status;
			if (status == CommentStatus.Violate)
			{
				cmt.Active = false;
			}

			_dbContext.Entry(cmt).State = EntityState.Modified;
			await _dbContext.SaveChangesAsync(cancellationToken);
		}

		return cmt;
	}
}

