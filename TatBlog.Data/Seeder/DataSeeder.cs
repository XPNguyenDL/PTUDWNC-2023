using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeder;

public class DataSeeder : IDataSeeder
{
    private readonly BlogDbContext _dbContext;

    public DataSeeder(BlogDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public void Initialize()
    {
        _dbContext.Database.EnsureCreated();

        if (_dbContext.Posts.Any())
        {
            return;
        }

        var authors = AddAuthors();
        var categories = AddCategories();
        var tags = AddTags();
        var posts = AddPosts(authors, categories, tags);

    }

    private IList<Author> AddAuthors()
    {
        var authors = new List<Author>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "Jason Mouth",
                UrlSlug = "jason-mouth",
                Email = "json@gmail.com",
                JoinedDate = new DateTime(2022, 10, 21)
            },
            new()
            {
                Id = Guid.NewGuid(),
                FullName = "Jessica Wonder",
                UrlSlug = "jessica-wonder",
                Email = "jessica665@motip.com",
                JoinedDate = new DateTime(2022, 4, 19)
            }
        };

        _dbContext.Authors.AddRange(authors);
        _dbContext.SaveChanges();
        return authors;
    }

    private IList<Category> AddCategories()
    {
        var categories = new List<Category>()
        {
            new() {Id = Guid.NewGuid(), Name = ".NET Core", Description = ".NET Core", UrlSlug = "net-core"},
            new() {Id = Guid.NewGuid(), Name = "Architecture", Description = "Architecture", UrlSlug = "architecture"},
            new() {Id = Guid.NewGuid(), Name = "Messaging", Description = "Messaging", UrlSlug = "messaging"},
            new() {Id = Guid.NewGuid(), Name = "OOP", Description = "Object-Oriented Program", UrlSlug = "oop"},
            new() {Id = Guid.NewGuid(), Name = "Design Patterns", Description = "Design Patterns", UrlSlug = "design-patterns"},
        };

        _dbContext.Categories.AddRange(categories);
        _dbContext.SaveChanges();
        return categories;
    }

    private IList<Tag> AddTags()
    {
        var tags = new List<Tag>()
        {
            new() {Id = Guid.NewGuid(), Name = "Google", Description = "Google Application", UrlSlug = "google"},
            new() {Id = Guid.NewGuid(), Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "asp-net-mvc"},
            new() {Id = Guid.NewGuid(), Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor-page"},
            new() {Id = Guid.NewGuid(), Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"},
            new() {Id = Guid.NewGuid(), Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep-learning"},
            new() {Id = Guid.NewGuid(), Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural-network"}
            };

        _dbContext.Tags.AddRange(tags);
        _dbContext.SaveChanges();
        return tags;
    }

    private IList<Post> AddPosts(IList<Author> authors, IList<Category> categories, IList<Tag> tags)
    {
        var posts = new List<Post>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "ASP.NET Core Diagnostic Scenarios",
                ShortDescription = "David and friend has great repository filled",
                Description = "Here's a few great DON'T and DO example",
                Meta = "David and friend has great repository filled",
                UrlSlug = "aspnet-core-diagnostic-scenarios",
                Published = true,
                PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                ModifiedDate = null,
                ViewCount = 10,
                Author = authors[0],
                Category = categories[0],
                Tags = new List<Tag>()
                {
                    tags[0]
                }
            }
        };


        _dbContext.Posts.AddRange(posts);
        _dbContext.SaveChanges();
        return posts;
    }

}