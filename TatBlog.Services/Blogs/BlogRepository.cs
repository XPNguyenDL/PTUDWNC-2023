using Microsoft.EntityFrameworkCore;
using SlugGenerator;
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

    public async Task<Post> GetPostAsync(int year, int month, int day, string slug, CancellationToken cancellationToken = default)
    {
        IQueryable<Post> postsQuery = _dbContext.Set<Post>()
            .Include(x => x.Category)
            .Include(x => x.Author)
            .Include(p => p.Tags)
            .Include(p => p.Comments);

        if (year > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
        }
        if (month > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
        }
        if (day > 0)
        {
            postsQuery = postsQuery.Where(x => x.PostedDate.Day == day);
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

    public async Task<IList<AuthorItem>> GetAuthorAsync(int numAuthor, CancellationToken cancellationToken = default)
    {
        IQueryable<Author> author = _dbContext.Set<Author>();
        return await author
            .OrderBy(x => x.FullName)
            .Select(x => new AuthorItem
            {
                Id = x.Id,
                FullName = x.FullName,
                UrlSlug = x.UrlSlug,
                ImageUrl = x.ImageUrl,
                Email = x.Email,
                JoinedDate = x.JoinedDate,
                PostCount = x.Posts.Count(p => p.Published),
            })
            .OrderByDescending(s => s.PostCount)
            .Take(numAuthor)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<AuthorItem>> GetAuthorAsync(CancellationToken cancellationToken = default)
    {
        IQueryable<Author> author = _dbContext.Set<Author>();
        return await author
            .OrderBy(x => x.FullName)
            .Select(x => new AuthorItem
            {
                Id = x.Id,
                FullName = x.FullName,
                UrlSlug = x.UrlSlug,
                ImageUrl = x.ImageUrl,
                Email = x.Email,
                JoinedDate = x.JoinedDate,
                PostCount = x.Posts.Count(p => p.Published),
            })
            .OrderByDescending(s => s.PostCount)
            .ToListAsync(cancellationToken);
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
            .Include(x => x.Posts)
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
    public async Task<IList<MonthlyPostCountItem>> CountPostByMonth(int month, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext.Set<Post>()
            .GroupBy(s => new { s.PostedDate.Month, s.PostedDate.Year})
            .Select(p => new MonthlyPostCountItem()
            {
                Month = p.Key.Month,
                Year = p.Key.Year,
                PostCount = p.Count(x => x.Published)
            })
            .OrderByDescending(s => s.Year)
            .ThenByDescending(s => s.Month)
            .Take(month)
            .ToListAsync(cancellationToken);
        return result;

    }

    public async Task<Post> GetPostByIdAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Post>()
            .Include(s => s.Author)
            .Include(s => s.Tags)
            .Include(s => s.Category)
            .Include(s => s.Comments)
            .FirstOrDefaultAsync(s => s.Id.Equals(postId), cancellationToken);
    }

    public async Task<Post> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default)
    {
        // Check if the post already exists in the database
        var postExists = await _dbContext.Set<Post>().AnyAsync(s => s.Id == post.Id, cancellationToken);

        
        // Create an empty list of tags for a new post
        if (!postExists || post.Tags == null)
        {
            post.Tags = new List<Tag>();
        }
        // Load the tags for an existing post
        else if (post.Tags == null || post.Tags.Count == 0)
        {
            await _dbContext.Entry(post)
                .Collection(x => x.Tags)
                .LoadAsync(cancellationToken);
        }

        // Process the valid tags provided for the post
        var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => new
            {
                Name = x,
                Slug = x.GenerateSlug()
            })
            .GroupBy(x => x.Slug)
            .ToDictionary(g => g.Key, g => g.First().Name);

        foreach (var kv in validTags)
        {
            var tagExists = post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (tagExists) continue;

            // Get the existing tag or create a new one
            var tag = await GetTagBySlugAsync(kv.Key, cancellationToken) ?? new Tag()
            {
                Name = kv.Value,
                Description = kv.Value,
                UrlSlug = kv.Key,
                Posts = new List<Post>()
            };

            if (tag.Posts.All(p => p.Id != post.Id))
            {
                tag.Posts.Add(post);
            }

            post.Tags.Add(tag);
        }

        // Add or update the post in the database
        if (postExists)
        {
            _dbContext.Posts.Update(post);
        }
        else
        {
            _dbContext.Posts.Add(post);
        }

        var enries = _dbContext.ChangeTracker.Entries();
        // Save changes to the database
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
                        s.PostedDate.Day == postQuery.Day ||
                        s.PostedDate.Month == postQuery.Month ||
                        s.Tags.Any(t => t.Name.Contains(postQuery.TagName))).ToListAsync(cancellationToken);
    }

    private IQueryable<Post> FilterPost(IPostQuery condition)
    {
        
        int keyNumber = 0;
        var keyword = !string.IsNullOrWhiteSpace(condition.Keyword) ? condition.Keyword.ToLower() : "";
        int.TryParse(condition.Keyword, out keyNumber);

        IQueryable<Post> posts = _dbContext.Set<Post>()
            .Include(t => t.Tags)
            .Include(s => s.Author)
            .Include(c => c.Category)
            .WhereIf(condition.Published, s => s.Published)
            .WhereIf(condition.NonPublished, s => !s.Published)
            .WhereIf(condition.CategoryId != Guid.Empty, p => p.CategoryId == condition.CategoryId)
            .WhereIf(condition.AuthorId != Guid.Empty, p => p.AuthorId == condition.AuthorId)
            .WhereIf(!string.IsNullOrWhiteSpace(condition.AuthorSlug), p => p.Author.UrlSlug == condition.AuthorSlug)
            .WhereIf(!string.IsNullOrWhiteSpace(condition.CategorySlug),
                p => p.Category.UrlSlug == condition.CategorySlug)
            .WhereIf(!string.IsNullOrWhiteSpace(condition.TagSlug),
                p => p.Tags.Any(t => t.UrlSlug == condition.TagSlug))
            .WhereIf(!string.IsNullOrWhiteSpace(condition.PostSlug), p => p.UrlSlug == condition.PostSlug)
            .WhereIf(condition.Year > 0, p => p.PostedDate.Year == condition.Year)
            .WhereIf(condition.Month > 0, p => p.PostedDate.Month == condition.Month)
            .WhereIf(condition.Day > 0, p => p.PostedDate.Day == condition.Day)
            .WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), s =>
                s.Category.UrlSlug.ToLower().Contains(keyword) ||
                s.Author.UrlSlug.ToLower().Contains(keyword) ||
                s.Author.FullName.ToLower().Contains(keyword) ||
                s.PostedDate.Day == keyNumber ||
                s.PostedDate.Month == keyNumber ||
                s.PostedDate.Year == keyNumber ||
                s.Tags.Any(t => t.UrlSlug.ToLower().Contains(keyword) || t.Name.ToLower().Contains(keyword)));

        #region old filter

        //if (condition.Published)
        //{
        //    posts = posts.Where(s => s.Published);
        //}

        //if (condition.CategoryId != Guid.Empty)
        //{
        //    posts = posts.Where(s => s.CategoryId == condition.CategoryId);
        //}

        //if (condition.AuthorId != Guid.Empty)
        //{
        //    posts = posts.Where(s => s.AuthorId == condition.AuthorId);
        //}

        //if (!string.IsNullOrWhiteSpace(condition.CategorySlug))
        //{
        //    posts = posts.Where(x => x.Category.UrlSlug == condition.CategorySlug);
        //}

        //if (!string.IsNullOrWhiteSpace(condition.TagSlug))
        //{
        //    posts = posts.Where(x => x.Tags.Any(t => t.UrlSlug == condition.TagSlug));
        //}

        //if (!string.IsNullOrWhiteSpace(condition.AuthorSlug))
        //{
        //    posts = posts.Where(x => x.Author.UrlSlug == condition.AuthorSlug);
        //}

        //if (condition.Month > 0)
        //{
        //    posts = posts.Where(s => s.PostedDate.Month == condition.Month);
        //}

        //if (condition.Day > 0)
        //{
        //    posts = posts.Where(s => s.PostedDate.Day == condition.Day);
        //}

        //if (condition.Year > 0)
        //{
        //    posts = posts.Where(s => s.PostedDate.Year == condition.Year);
        //}


        //if (!string.IsNullOrWhiteSpace(condition.Keyword))
        //{
        //    posts = posts.Where(s => s.Category.UrlSlug.ToLower().Contains(keyword) ||
        //                             s.Author.UrlSlug.ToLower().Contains(keyword) ||
        //                             s.Author.FullName.ToLower().Contains(keyword) ||
        //                             s.PostedDate.Day == keyNumber ||
        //                             s.PostedDate.Month == keyNumber ||
        //                             s.Tags.Any(t => t.UrlSlug.ToLower().Contains(keyword) || t.Name.ToLower().Contains(keyword)));
        //}

        #endregion

        return posts;
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