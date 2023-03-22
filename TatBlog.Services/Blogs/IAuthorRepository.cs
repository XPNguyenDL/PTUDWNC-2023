using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IAuthorRepository
{
    Task<Author> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Author> GetCachedAuthorByIdAsync(Guid authorId);

    Task<IList<AuthorItem>> GetAuthorsAsync(
        CancellationToken cancellationToken = default);

    Task<int> CountAuthorAsync(CancellationToken cancellationToken = default);
    Task<bool> IsExistAuthorSlugAsync(Guid id, string slug, CancellationToken cancellationToken = default);

    Task<Author> GetAuthorBySlugAsync(string urlSlug, CancellationToken cancellationToken = default);

    Task<Author> GetCachedAuthorBySlugAsync(
        string slug, CancellationToken cancellationToken = default);

    Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IAuthorQuery condition, IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);

    Task<IPagedList<T>> GetPagedAuthorsAsync<T>(
        Func<IQueryable<Author>, IQueryable<T>> mapper,
        IPagingParams pagingParams,
        string name = null,
        CancellationToken cancellationToken = default);

    Task<Author> AddOrUpdateAuthor(Author author, CancellationToken cancellationToken = default);

    Task<List<Author>> GetAuthorMostPost(int authorNum, CancellationToken cancellationToken = default);

    Task<bool> DeleteAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default);


    Task<bool> SetImageUrlAsync(
        Guid authorId, string imageUrl,
        CancellationToken cancellationToken = default);
}