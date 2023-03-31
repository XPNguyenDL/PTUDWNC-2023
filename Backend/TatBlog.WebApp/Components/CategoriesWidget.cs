using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

public class CategoriesWidget : ViewComponent
{
    private readonly IBlogRepository _blog;

    public CategoriesWidget(IBlogRepository blog)
    {
        _blog = blog;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // lấy danh sách chủ đề
        var categories = await _blog.GetCategoriesAsync();
        return View(categories);
    }
}