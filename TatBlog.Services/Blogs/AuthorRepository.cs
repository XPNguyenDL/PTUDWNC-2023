using Microsoft.EntityFrameworkCore;
using System.Linq;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class AuthorRepository : IAuthorRepository
{
    private readonly BlogDbContext _dbContext;

    public AuthorRepository(BlogDbContext context)
    {
        _dbContext = context;
    }
    public async Task<Author> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Author>()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<int> CountAuthorAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Author>().CountAsync(cancellationToken);
    }

    public async Task<bool> IsExistAuthorSlugAsync(Guid id, string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Author>().AnyAsync(s => s.Id != id && s.UrlSlug.Equals(slug), cancellationToken);
    }

    public async Task<Author> GetAuthorBySlugAsync(string urlSlug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Author>()
            .FirstOrDefaultAsync(s => s.UrlSlug == urlSlug, cancellationToken);
    }

    private IQueryable<AuthorItem> AuthorFilter(IAuthorQuery condition)
    {
        var authors = _dbContext.Set<Author>()
            .WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), s => 
                s.Email.Contains(condition.Keyword) ||
                s.FullName.Contains(condition.Keyword) ||
                s.Notes.Contains(condition.Keyword))
            .WhereIf(condition.Month != 0, s => s.JoinedDate.Month == condition.Month)
            .WhereIf(condition.Year != 0, s => s.JoinedDate.Year == condition.Year)
            .Select(s => new AuthorItem()
            {
                Id = s.Id,
                Email = s.Email,
                FullName = s.FullName,
                ImageUrl = s.ImageUrl,
                JoinedDate = s.JoinedDate,
                Notes = s.Notes,
                PostCount = s.Posts.Count(p => p.Published)
            });
        return authors;
    }

    public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IAuthorQuery condition, IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        return await AuthorFilter(condition).ToPagedListAsync(pagingParams, cancellationToken);
    }

    public async Task<Author> AddOrUpdateAuthor(Author author, CancellationToken cancellationToken = default)
    {
        if (_dbContext.Set<Author>().Any(s => s.Id == author.Id))
        {
            _dbContext.Entry(author).State = EntityState.Modified;
        }
        else
        {
            _dbContext.Authors.Add(author);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return author;
    }

    public async Task<List<Author>> GetAuthorMostPost(int authorNum, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Author>()
            .OrderByDescending(s => s.Posts.Count(p => p.Published)).Take(authorNum).ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Author>()
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }
}