using System.ComponentModel;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.Entities;

namespace TatBlog.WebApp.Areas.Admin.Models;

public class CommentFilterModel
{
    [DisplayName("Từ khóa")]
    public string Keyword { get; set; }

    [DisplayName("Năm")]
    public int? Year { get; set; }

    [DisplayName("Tháng")]
    public int? Month { get; set; }

    public CommentStatus CommentStatus { get; set; } = CommentStatus.None;

    public bool Active { get; set; }

    public IEnumerable<SelectListItem>? MonthList { get; set; }

    public CommentFilterModel()
    {
        MonthList = Enumerable.Range(1, 12)
            .Select(m => new SelectListItem()
            {
                Value = m.ToString(),
                Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m),
            }).ToList();
    }
}