using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Services.Blogs;

public class CommentRepository : ICommentRepository
{
    private readonly BlogDbContext _dbContext;

    public CommentRepository(BlogDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Comment> AddOrUpdateCommentAsync(Comment comment, CancellationToken cancellationToken = default)
    {
        if (_dbContext.Set<Comment>().Any(s => s.Id == comment.Id))
        {
            _dbContext.Entry(comment).State = EntityState.Modified;
        }
        else
        {
            _dbContext.Comments.Add(comment);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return comment;
    }
    public async Task<bool> DeleteCommentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Comment>()
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken) > 0;
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

