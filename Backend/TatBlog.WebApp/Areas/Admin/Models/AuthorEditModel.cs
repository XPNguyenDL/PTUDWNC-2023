﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace TatBlog.WebApp.Areas.Admin.Models;

public class AuthorEditModel
{
    public Guid Id { get; set; }

    public string? FullName { get; set; }

    public string? UrlSlug { get; set; }

    public string? Email { get; set; }

    public string? Notes { get; set; }
    
    public string? ImageUrl { get; set; }

    public IFormFile? ImageFile { get; set; }
    

}