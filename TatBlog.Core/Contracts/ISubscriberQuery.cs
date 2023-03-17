using System.ComponentModel;
using TatBlog.Core.Entities;

namespace TatBlog.Core.Contracts;

public interface ISubscriberQuery
{
    [DisplayName("Từ khóa")]
    public string Keyword { get; set; }

    [DisplayName("Năm")]
    public int? Year { get; set; }

    [DisplayName("Tháng")]
    public int? Month { get; set; }
    
    public SubscribeStatus Status { get; set; }
}