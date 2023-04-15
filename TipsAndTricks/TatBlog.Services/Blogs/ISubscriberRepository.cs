using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface ISubscriberRepository
{
    Task<bool> SubscribeAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> UnSubscribeAsync(string email, string reason, CancellationToken cancellationToken = default);
    Task<bool> BlockSubscribeAsync(Guid id, string reason, string notes, CancellationToken cancellationToken = default);
    Task<Subscriber> GetSubscriberByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> DeleteSubscriberAsync(Guid id, CancellationToken cancellationToken = default);
    Task<int> CountSubscriberByDayAsync(CancellationToken cancellationToken = default);
    Task<int> CountSubscriberAsync(CancellationToken cancellationToken = default);

    Task<Subscriber> GetSubscriberByEmail(string email, CancellationToken cancellationToken = default);
    Task<IPagedList<Subscriber>> SearchSubscribersAsync(ISubscriberQuery condition, IPagingParams pagingParams, CancellationToken cancellation = default);
}