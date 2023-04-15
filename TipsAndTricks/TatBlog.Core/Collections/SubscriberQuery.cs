using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Core.Collections;

public class SubscriberQuery : ISubscriberQuery
{
    public string Keyword { get; set; } = "";
    public string Email { get; set; } = "";
    public int? Year { get; set; } = 0;
    public int? Month { get; set; } = 0;
    public SubscribeStatus Status { get; set; } = SubscribeStatus.None;
}