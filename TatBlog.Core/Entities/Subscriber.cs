using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities;

public enum SubscribeStatus
{
    Subscribe,
    Unsubscribe,
    Block
}

public class Subscriber : IEntity
{
    public Guid Id { get; set; }

    public string Email { get; set; }

    public DateTime DateSubscribe { get; set; }

    public DateTime? DateUnSubscribe { get; set; }

    public string Reason { get; set; }

    public SubscribeStatus SubscribeStatus { get; set; }

    public string Note { get; set; }

}