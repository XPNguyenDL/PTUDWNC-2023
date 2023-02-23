using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _dbContext;

    public BlogRepository(BlogDbContext context)
    {
        _dbContext = context;
    }

    public async Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postsQuery = _dbContext.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author);
        if (year > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
        }
        if (month > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
        }
        if (!string.IsNullOrWhiteSpace(slug))
        {
            postsQuery = postsQuery.Where(x => x.UrlSlug.Equals(slug));
        }

        return await postsQuery.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<Post>> GetPopularArticlesAsync(int numPosts, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Post>()
            .Include(p => p.Category)
            .Include(p => p.Author)
            .OrderByDescending(p => p.ViewCount)
            .Take(numPosts)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> IsPostSlugExistedAsync(Guid postId, string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Post>()
            .AnyAsync(s => s.Id != postId && s.UrlSlug.Equals(slug), cancellationToken);
    }

    public async Task IncreaseViewCountAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<Post>()
            .Where(x => x.Id == postId)
            .ExecuteUpdateAsync(p => p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
    }

    public async Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default)
    {
        IQueryable<Category> categories = _dbContext.Set<Category>();

        if (showOnMenu)
        {
            categories = categories.Where(s => s.ShowOnMenu);
        }

        return await categories
            .OrderBy(x => x.Name)
            .Select(x => new CategoryItem
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                ShowOnMenu = x.ShowOnMenu,
                PostCount = x.Posts.Count(p => p.Published),
            }).ToListAsync(cancellationToken);
    }

    public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
        IPagingParams pagingParams,
        CancellationToken cancellationToken = default)
    {
        var tagQuery = _dbContext.Set<Tag>()
            .Select(x => new TagItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                PostCount = x.Posts.Count(p => p.Published)
            });

        return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
    }
}