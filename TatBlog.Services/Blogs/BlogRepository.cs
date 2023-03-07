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

    // Lấy thẻ tag theo tên (slug)

    public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Tag>()
            .FirstOrDefaultAsync(t => t.UrlSlug.Equals(slug), cancellationToken);
    }

    public async Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Tag>()
            .Select(s => new TagItem()
            {
                Id = s.Id,
                Name = s.Name,
                UrlSlug = s.UrlSlug,
                Description = s.Description,
                PostCount = s.Posts.Count(p => p.Published)
            }).ToListAsync(cancellationToken);
    }

    public async Task<bool> DeleteTagByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var tag = _dbContext.Set<Tag>().FirstOrDefault(t => t.Id == id);
        if (tag != null)
        {
            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Category>()
            .FirstOrDefaultAsync(t => t.UrlSlug.Equals(slug), cancellationToken);
    }

    public async Task<Category> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Category>()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Category> AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
    {
        if (_dbContext.Set<Category>().Any(s => s.Id == category.Id))
        {
            _dbContext.Entry(category).State = EntityState.Modified;
        }
        else
        {
            _dbContext.Categories.Add(category);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return category;
    }

    public async Task<bool> DeleteCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Category>()
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<bool> IsCategorySlugExistedAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Category>().AnyAsync(s => s.UrlSlug.Equals(slug), cancellationToken);
    }

    public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default)
    {
        var tagQuery = _dbContext.Set<Category>()
            .Select(x => new CategoryItem()
            {
                Id = x.Id,
                Name = x.Name,
                UrlSlug = x.UrlSlug,
                Description = x.Description,
                ShowOnMenu = x.ShowOnMenu,
                PostCount = x.Posts.Count(p => p.Published)
            });

        return await tagQuery.ToPagedListAsync(pagingParams, cancellationToken);
    }

    // Còn bug 
    public async Task<(int day, int month, int PostCount)> CountPostByMonth(int month, CancellationToken cancellationToken = default)
    {
        var date = DateTime.Now.AddMonths(-month);
        var result = await _dbContext.Set<Post>()
            .Where(s => s.PostedDate > date).CountAsync(cancellationToken);
        return (date.Day, month, result);

    }

    public async Task<Post> GetPostByIdAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Post>().FirstOrDefaultAsync(s => s.Id.Equals(postId), cancellationToken);
    }

    public async Task<Post> AddOrUpdatePostAsync(Post post, CancellationToken cancellationToken = default)
    {
        if (_dbContext.Set<Post>().Any(s => s.Id == post.Id))
        {
            _dbContext.Entry(post).State = EntityState.Modified;
        }
        else
        {
            _dbContext.Posts.Add(post);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return post;
    }

    public async Task TogglePublicStatusPostAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<Post>()
            .Where(x => x.Id == postId)
            .ExecuteUpdateAsync(p => p.SetProperty(x => x.Published, x => !x.Published), cancellationToken);
    }

    public async Task<IList<Post>> GetRandomPostAsync(int ranNum, CancellationToken cancellation = default)
    {
        return await _dbContext.Set<Post>()
            .Include(s => s.Author)
            .Include(c => c.Category)
            .OrderBy(c => Guid.NewGuid())
            .Take(ranNum).ToListAsync(cancellation);
    }

    public async Task<IList<Post>> FindPostsQueryAsync(IPostQuery postQuery, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Post>()
            .Include(s => s.Author)
            .Include(c => c.Category)
            .Where(s => s.Author.Id == postQuery.AuthorId ||
                        s.CategoryId == postQuery.CategoryId ||
                        s.Category.UrlSlug == postQuery.CategorySlug ||
                        s.PostedDate.Day == postQuery.CreatedDate.Day ||
                        s.PostedDate.Month == postQuery.CreatedDate.Month ||
                        s.Tags.Any(t => t.Name.Contains(postQuery.TagName))).ToListAsync(cancellationToken);
    }

    private IQueryable<Post> FilterPost(IPostQuery postQuery)
    {
        return _dbContext.Set<Post>()
            .Include(t => t.Tags)
            .Include(s => s.Author)
            .Include(c => c.Category)
            .Where(s => s.Author.Id == postQuery.AuthorId ||
                        s.CategoryId == postQuery.CategoryId ||
                        s.Category.UrlSlug == postQuery.CategorySlug ||
                        s.PostedDate.Day == postQuery.CreatedDate.Day ||
                        s.PostedDate.Month == postQuery.CreatedDate.Month ||
                        s.Tags.Any(t => t.Name.Contains(postQuery.TagName)));
    }

    public async Task<int> CountPostsQueryAsync(IPostQuery postQuery, CancellationToken cancellationToken = default)
    {
        return await FilterPost(postQuery).CountAsync(cancellationToken);
    }

    public async Task<IPagedList<Post>> GetPagedPostsQueryAsync(IPostQuery postQuery, 
        int pageNumber = 1,
        int pageSize = 10, 
        CancellationToken cancellationToken = default)
    {

        return await FilterPost(postQuery).ToPagedListAsync(pageNumber, pageSize, nameof(Post.PostedDate), "DESC", cancellationToken);
    }

    // t chưa làm được

}