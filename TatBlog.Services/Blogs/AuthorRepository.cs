using Microsoft.EntityFrameworkCore;
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

    public async Task<Author> GetAuthorBySlugAsync(string urlSlug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Author>()
            .FirstOrDefaultAsync(s => s.UrlSlug == urlSlug, cancellationToken);
    }

    public async Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        var authorQuery = _dbContext.Set<Author>()
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
        return await authorQuery.ToPagedListAsync(pagingParams, cancellationToken);
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
}