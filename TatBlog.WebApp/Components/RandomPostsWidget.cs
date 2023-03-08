﻿using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Components;

public class RandomPostsWidget : ViewComponent
{
    private readonly IBlogRepository _blog;

    public RandomPostsWidget(IBlogRepository blog)
    {
        _blog = blog;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        // lấy danh sách chủ đề
        var posts = await _blog.GetRandomPostAsync(5);
        return View(posts);
    }
    
}