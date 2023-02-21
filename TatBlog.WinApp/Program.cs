

using TatBlog.Data.Contexts;
using TatBlog.Data.Seeder;

var conext = new BlogDbContext();
var seeder = new DataSeeder(conext);
seeder.Initialize();

var authors = conext.Authors.ToList();

Console.WriteLine("{0, -40}{1, -30}{2, -30}{3, 12:MM/dd/yyyy}",
    "ID", "Full Name", "Email", "Joined Date");

foreach (var author in authors)
{
    Console.WriteLine("{0, -40}{1, -30}{2, -30}{3, 12:MM/dd/yyyy}",
        author.Id, author.FullName, author.Email, author.JoinedDate);
}

