
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

namespace TatBlog.Services.Blogs;

public class SubscriberRepository : ISubscriberRepository
{
    private readonly BlogDbContext _dbContext;

    public SubscriberRepository(BlogDbContext context)
    {
        _dbContext = context;
    }

    public async Task<bool> SubscribeAsync(string email, CancellationToken cancellationToken)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
        {
            var sub = _dbContext.Set<Subscriber>().FirstOrDefault(s => s.Email.Equals(email));
            if (sub == null)
            {
                var newSub = new Subscriber()
                {
                    Id = Guid.NewGuid(),
                    DateSubscribe = DateTime.Now,
                    Email = email,
                    SubscribeStatus = SubscribeStatus.Subscribe
                };
                _dbContext.Subscribers.Add(newSub);
            }
            else
            {
                if (sub.SubscribeStatus == SubscribeStatus.Block || sub.SubscribeStatus == SubscribeStatus.Subscribe) return false;
                sub.DateSubscribe = DateTime.Now;
                sub.DateUnSubscribe = null;
                sub.SubscribeStatus = SubscribeStatus.Subscribe;
                sub.Reason = null;
                _dbContext.Entry(sub).State = EntityState.Modified;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        return false;
    }

    public async Task<bool> UnSubscribeAsync(string email, string reason, CancellationToken cancellationToken = default)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
        {
            var subscriber = _dbContext.Set<Subscriber>().FirstOrDefault(s => s.Email.Equals(email));
            if (subscriber != null && subscriber.SubscribeStatus == SubscribeStatus.Subscribe)
            {
                subscriber.Reason = reason;
                subscriber.SubscribeStatus = SubscribeStatus.Unsubscribe;
                subscriber.DateUnSubscribe = DateTime.Now;
                _dbContext.Entry(subscriber).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync(cancellationToken);
                return true;
            }
        }
        return false;
    }

    public async Task<bool> BlockSubscribeAsync(Guid id, string reason, string notes, CancellationToken cancellationToken = default)
    {
        var subscriber = _dbContext.Set<Subscriber>().FirstOrDefault(s => s.Id.Equals(id));
        if (subscriber != null)
        {
            subscriber.Reason = reason;
            subscriber.Note = notes;
            subscriber.SubscribeStatus = SubscribeStatus.Block;
            subscriber.DateUnSubscribe = DateTime.Now;
            _dbContext.Entry(subscriber).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        return false;
    }

    public async Task<Subscriber> GetSubscriberByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Subscriber>().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<bool> DeleteSubscriberAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Subscriber>()
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken) > 0;
    }

    public async Task<int> CountSubscriberByDayAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Subscriber>().CountAsync(s => s.DateSubscribe > DateTime.Now.AddDays(-1), cancellationToken);
    }
    
    public async Task<int> CountSubscriberAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Subscriber>().CountAsync(cancellationToken);
    }

    public async Task<Subscriber> GetSubscriberByEmail(string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<Subscriber>().FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
    }

    public async Task<IPagedList<Subscriber>> SearchSubscribersAsync(ISubscriberQuery condition, IPagingParams pagingParams, CancellationToken cancellation = default)
    {
        var subQuery = _dbContext.Set<Subscriber>()
            .WhereIf(!string.IsNullOrWhiteSpace(condition.Keyword), s => s.Email.Contains(condition.Keyword) ||
                                                                        s.Note.ToLower().Contains(condition.Keyword.ToLower()) ||
                                                                        s.Reason.ToLower().Contains(condition.Keyword.ToLower()))
            .WhereIf(condition.Month > 0 || condition.Year > 0, s => s.DateSubscribe.Month == condition.Month ||
                                                                     s.DateSubscribe.Year == condition.Year)
            .WhereIf(condition.Status != SubscribeStatus.None, s => s.SubscribeStatus == condition.Status);
        return await subQuery.ToPagedListAsync(pagingParams, cancellation);
    }
}