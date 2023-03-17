using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
    Task<Post> GetPostAsync(int year, int month, int day, string slug, CancellationToken cancellationToken = default);
    Task<int> CountPostAsync(CancellationToken cancellationToken = default);
    Task<int> CountPostUnPublicAsync(CancellationToken cancellationToken = default);


    // Tìm bài viết nhiều bài nhất
    Task<IList<Post>> GetPopularArticlesAsync(
        int numPosts,
        CancellationToken cancellationToken = default);

    // Kiểm tra tên định danh
    Task<bool> IsPostSlugExistedAsync(
        Guid postId,
        string slug,
        CancellationToken cancellationToken = default);

    // Increase view post
    Task IncreaseViewCountAsync(Guid postId, CancellationToken cancellationToken = default);

    Task<IList<CategoryItem>> GetCategoriesAsync(bool showOnMenu = false, CancellationToken cancellationToken = default);
    Task<IList<AuthorItem>> GetAuthorAsync(int numAuthor, CancellationToken cancellationToken = default);
    Task<IList<AuthorItem>> GetAuthorAsync(CancellationToken cancellationToken = default);

    Task<IPagedList<TagItem>> GetPagedTagsAsync(string keyword, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    // Get Tag by slug
    Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Tag> GetTagByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Tag> AddOrUpdateTagAsync(Tag tag, CancellationToken cancellationToken = default);
    Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteTagByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeletePostByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Category> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category> AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsCategorySlugExistedAsync(
        Guid id,
        string slug,
        CancellationToken cancellationToken = default);
    Task<bool> IsTagSlugExistedAsync(
        Guid id,
        string slug,
        CancellationToken cancellationToken = default);
    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(ICategoryQuery categoryQuery, IPagingParams pagingParams, CancellationToken cancellationToken = default);
    Task<IList<MonthlyPostCountItem>> CountPostByMonth(int month, CancellationToken cancellationToken = default);

    Task<Post> GetPostByIdAsync(Guid postId, bool includeDetails = false,
        CancellationToken cancellationToken = default);
    Task<Post> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default);

    Task TogglePublicStatusPostAsync(Guid postId, CancellationToken cancellation = default);

    Task<IList<Post>> GetRandomPostAsync(int ranNum, CancellationToken cancellation = default);

    Task<IList<Post>> FindPostsQueryAsync(IPostQuery postQuery, CancellationToken cancellationToken = default);
    Task<int> CountPostsQueryAsync(IPostQuery postQuery, CancellationToken cancellationToken = default);
    Task<IPagedList<Post>> GetPagedPostsQueryAsync(IPostQuery postQuery,
        int pageNumber = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default);
}