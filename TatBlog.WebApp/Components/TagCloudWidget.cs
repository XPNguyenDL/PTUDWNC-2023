using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

public class TagCloudWidget : ViewComponent
{
    private readonly IBlogRepository _blog;

    public TagCloudWidget(IBlogRepository blog)
    {
        _blog = blog;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // lấy danh sách chủ đề
        var tagCloud = await _blog.GetTagsAsync();
        return View(tagCloud);
    }
}