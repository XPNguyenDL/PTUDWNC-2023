using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

public class ArchivesWidget : ViewComponent
{
    private readonly IBlogRepository _blog;

    public ArchivesWidget(IBlogRepository blog)
    {
        _blog = blog;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // lấy danh sách chủ đề
        var monthlyPost = await _blog.CountPostByMonth(12);
        return View(monthlyPost);
    }
}