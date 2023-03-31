namespace TatBlog.WebApp.Areas.Admin.Models;

public class SubscriberEditModel
{
    public Guid Id { get; set; }
    public string? Reason { get; set; }
    public string? Note { get; set; }
}