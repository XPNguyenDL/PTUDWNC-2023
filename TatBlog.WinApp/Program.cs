

using TatBlog.Data.Contexts;
using TatBlog.Data.Seeder;

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

#region MyRegion

var posts = context.Posts.Where(p => p.Published)
    .OrderBy(p => p.Title)
    .Select(p => new
    {
        Id = p.Id,
        Title = p.Title,
        ViewCount = p.ViewCount,
        PostedDate = p.PostedDate,
        Author = p.Author.FullName,
        Category = p.Category.Name
    }).ToList();

foreach (var post in posts)
{
    Console.WriteLine("Id: {0}", post.Id);
    Console.WriteLine("Title: {0}", post.Title);
    Console.WriteLine("ViewCount: {0}", post.ViewCount);
    Console.WriteLine("PostedDate: {0}", post.PostedDate);
    Console.WriteLine("Author: {0}", post.Author);
    Console.WriteLine("Category: {0}", post.Category);
    Console.WriteLine("".PadRight(80, '-'));
}
#endregion