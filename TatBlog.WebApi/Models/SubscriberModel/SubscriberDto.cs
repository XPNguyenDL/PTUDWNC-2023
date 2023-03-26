using TatBlog.Core.Entities;

namespace TatBlog.WebApi.Models.SubscriberModel;

public class SubscriberDto
{
	public Guid Id { get; set; }
	public string Email { get; set; }

	public DateTime DateSubscribe { get; set; }

	public DateTime? DateUnSubscribe { get; set; }

	public string Reason { get; set; }

	public SubscribeStatus SubscribeStatus { get; set; }
}