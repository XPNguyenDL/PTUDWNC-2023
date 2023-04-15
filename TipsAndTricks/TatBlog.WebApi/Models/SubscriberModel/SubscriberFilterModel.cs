namespace TatBlog.WebApi.Models.SubscriberModel;

public class SubscriberFilterModel : PagingModel
{
	public string Keyword { get; set; }
	public string Email { get; set; }
}