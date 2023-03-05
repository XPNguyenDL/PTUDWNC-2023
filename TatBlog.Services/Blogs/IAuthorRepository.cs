using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;

namespace TatBlog.Services.Blogs;

public interface IAuthorRepository
{
    Task<Author> GetAuthorByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Author> GetAuthorBySlugAsync(string urlSlug, CancellationToken cancellationToken = default);

    Task<IPagedList<AuthorItem>> GetPagedAuthorsAsync(IPagingParams pagingParams, CancellationToken cancellationToken = default);

    Task<Author> AddOrUpdateAuthor(Author author, CancellationToken cancellationToken = default);

    Task<List<Author>> GetAuthorMostPost(int authorNum, CancellationToken cancellationToken = default);
}