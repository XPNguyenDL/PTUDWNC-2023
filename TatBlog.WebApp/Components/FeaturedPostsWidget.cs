using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

public class FeaturedPostsWidget : ViewComponent
{
    private readonly IBlogRepository _blog;

    public FeaturedPostsWidget(IBlogRepository blog)
    {
        _blog = blog;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // lấy danh sách chủ đề
        var posts = await _blog.GetPopularArticlesAsync(3);
        return View(posts);
    }
    
}