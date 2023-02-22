

using Microsoft.Identity.Client;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeder;
using TatBlog.Services.Blogs;

var context = new BlogDbContext();
var seeder = new DataSeeder(context);
seeder.Initialize();

#region Show Authors

//var authors = conext.Authors.ToList();

//Console.WriteLine("{0, -40}{1, -30}{2, -30}{3, 12:MM/dd/yyyy}",
//    "ID", "Full Name", "Email", "Joined Date");

//foreach (var author in authors)
//{
//    Console.WriteLine("{0, -40}{1, -30}{2, -30}{3, 12:MM/dd/yyyy}",
//        author.Id, author.FullName, author.Email, author.JoinedDate);
//}

#endregion

#region Show post

//var posts = context.Posts.Where(p => p.Published)
//    .OrderBy(p => p.Title)
//    .Select(p => new
//    {
//        Id = p.Id,
//        Title = p.Title,
//        ViewCount = p.ViewCount,
//        PostedDate = p.PostedDate,
//        Author = p.Author,
//        Category = p.Category
//    }).ToList();


#endregion

#region Blog repository

IBlogRepository blogRepo = new BlogRepository(context);

var posts = await blogRepo.GetPopularArticlesAsync(3);

//PrintPosts(posts);

var category = await blogRepo.GetCategoriesAsync();

PrintCategories(category);




#endregion


void PrintPosts(IList<Post> posts)
{
    foreach (var post in posts)
    {
        Console.WriteLine("Id: {0}", post.Id);
        Console.WriteLine("Title: {0}", post.Title);
        Console.WriteLine("ViewCount: {0}", post.ViewCount);
        Console.WriteLine("PostedDate: {0}", post.PostedDate);
        Console.WriteLine("Author: {0}", post.Author.FullName);
        Console.WriteLine("Category: {0}", post.Category.Name);
        Console.WriteLine("".PadRight(80, '-'));
    }
}

void PrintCategories(IList<CategoryItem> categories)
{
    Console.WriteLine("{0, -40}{1, -50}{2, 10}",
        "ID", "Name", "Count");

    foreach (var category in categories)
    {
        Console.WriteLine("{0, -40}{1, -50}{2, 10}",
            category.Id, category.Name, category.PostCount);
    }
}