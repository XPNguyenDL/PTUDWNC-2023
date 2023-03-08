using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

public class BestAuthorsWidget : ViewComponent
{
    private readonly IBlogRepository _blog;

    public BestAuthorsWidget(IBlogRepository blog)
    {
        _blog = blog;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // lấy danh sách chủ đề
        var authors = await _blog.GetAuthorAsync(4);
        return View(authors);
    }
}