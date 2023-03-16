using System.ComponentModel;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TatBlog.WebApp.Areas.Admin.Models;

public class AuthorFilterModel
{
    [DisplayName("Từ khóa")]
    public string Keyword { get; set; }

    [DisplayName("Năm")]
    public int? Year { get; set; }

    [DisplayName("Tháng")]
    public int? Month { get; set; }

    public IEnumerable<SelectListItem>? MonthList { get; set; }

    public AuthorFilterModel()
    {
        MonthList = Enumerable.Range(1, 12)
            .Select(m => new SelectListItem()
            {
                Value = m.ToString(),
                Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m),
            }).ToList();
    }

}