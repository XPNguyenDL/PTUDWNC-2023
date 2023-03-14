

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.Entities;

namespace TatBlog.WebApp.Areas.Admin.Models;

public class PostEditModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    
    public string ShortDescription { get; set; }
    
    public string Description { get; set; }

    // Meta data
    public string Meta { get; set; }
    
    public string UrlSlug { get; set; }
    
    public IFormFile? ImageFile { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public bool Published { get; set; }
    
    public Guid CategoryId { get; set; }
    
    public Guid AuthorId { get; set; }
    
    public string SelectedTags { get; set; }

    public IEnumerable<SelectListItem>? AuthorList { get; set; }
    public IEnumerable<SelectListItem>? CategoryList { get; set; }

    public List<string> GetSelectTags()
    {
        return (SelectedTags ?? "")
            .Split(new[] { ',', ';', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

}