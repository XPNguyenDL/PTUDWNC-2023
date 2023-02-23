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
}