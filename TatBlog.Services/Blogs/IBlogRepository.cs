using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IBlogRepository
{
    Task<Post> GetPostAsync(int year, int month, string slug, CancellationToken cancellationToken = default);

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

    Task<IPagedList<TagItem>> GetPagedTagsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
    
    // Get Tag by slug
    Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<IList<TagItem>> GetTagsAsync(CancellationToken cancellationToken = default);
    Task<bool> DeleteTagByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<Category> GetCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Category> AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default);
    Task<bool> DeleteCategoryByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> IsCategorySlugExistedAsync(
        string slug,
        CancellationToken cancellationToken = default);
    Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);
    Task<(int day, int month, int PostCount)> CountPostByMonth(int month, CancellationToken cancellationToken = default);
}