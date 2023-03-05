

using Microsoft.Identity.Client;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeder;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

var context = new BlogDbContext();
var seeder = new DataSeeder(context);
seeder.Initialize();

IBlogRepository blogRepo = new BlogRepository(context);
IAuthorRepository authorRepo = new AuthorRepository(context);
ISubscriberRepository subRepo = new SubscriberRepository(context);

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

//IBlogRepository blogRepo = new BlogRepository(context);

//var posts = await blogRepo.GetPopularArticlesAsync(3);

////PrintPosts(posts);

//var category = await blogRepo.GetCategoriesAsync();

//PrintCategories(category);

//subRepo.UnSubscribeAsync("2014478@dlu.edu.vn", "Hủy đăng ký");

var paringParams = new PagingParams()
{
    PageNumber = 1,
    PageSize = 5,
    SortColumn = "Email",
    SortOrder = "DESC"
};

var subs = await subRepo.SearchSubscribersAsync(paringParams, "2014478", SubscribeStatus.Block);

foreach (var sub in subs)
{

    Console.WriteLine("{0, -40}{1, -30}{2, -30}{3, 12}", sub.Id, sub.Email, sub.Reason, sub.Note);
}
#endregion

#region Phân trang
//var paringParams = new PagingParams()
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "PostCount",
//    SortOrder = "DESC"
//};

//var tagsList = await blogRepo.GetPagedTagsAsync(paringParams);

//Console.WriteLine("{0, -40}{1, -50}{2, 10}", "Id", "Name", "Count");

//foreach (var tagItem in tagsList)
//{
//    Console.WriteLine("{0, -40}{1, -50}{2, 10}", tagItem.Id, tagItem.Name, tagItem.PostCount);
//}



//PostQuery postQuery = new()
//{
//    CategoryId = Guid.Parse("885771be-ef28-4c85-a896-5919ecca366e")
//};

//var postList = await blogRepo.GetPagedPostsQueryAsync(paringParams, postQuery);

//foreach (var post in postList)
//{
//    Console.WriteLine("{0, -40}{1, -50}{2, 10}", post.Id, post.Title, post.Author.FullName);
//}


#endregion

#region Bài tập


// c.Lấy danh sách tất cả các thẻ (Tag) kèm theo số bài viết chứa thẻ đó. Kết 
// quả trả về kiểu IList<TagItem>.

//var tagList = await blogRepo.GetTagsAsync();

//foreach (var tagItem in tagList)
//{
//    Console.WriteLine("{0, -40}{1, -50}{2, 10}", tagItem.Id, tagItem.Name, tagItem.PostCount);
//}

// d. Xóa một thẻ theo mã cho trước. 

//await blogRepo.DeleteTagByIdAsync(Guid.Parse("9fdc3139-b1fb-483a-8df9-4d993d242035"));

//var tagList = await blogRepo.GetTagsAsync();


//foreach (var tagItem in tagList)
//{
//    Console.WriteLine("{0, -40}{1, -50}{2, 10}", tagItem.Id, tagItem.Name, tagItem.PostCount);
//}

// e. Tìm một chuyên mục (Category) theo tên định danh (slug). 

//var category = await blogRepo.GetCategoryBySlugAsync("net-core");
//Console.WriteLine("{0, -40}{1, -50}{2, 10}",
//    category.Id, category.Name, category.UrlSlug);

//f. Tìm một chuyên mục theo mã số cho trước

//var newCategory = new Category()
//{
//    Id = Guid.Parse("bcca0f65-4ed9-4898-a160-08db1b1030e1"),
//    Name = "GitLab 1",
//    UrlSlug = "git-lab-1"
//};

//var category = await blogRepo.AddOrUpdateCategoryAsync(newCategory);

//Console.WriteLine("{0, -40}{1, -50}{2, 10}",
//    category.Id, category.Name, category.UrlSlug);

//var categories = await blogRepo.GetCategoriesAsync();
//PrintCategories(categories);

//h. Xóa một chuyên mục theo mã số cho trước. 

//await blogRepo.DeleteCategoryByIdAsync(Guid.Parse("a78f98e2-0c4f-41dc-47da-08db1b1079dd"));
//var categories = await blogRepo.GetCategoriesAsync();
//PrintCategories(categories);

// i. Kiểm tra tên định danh (slug) của một chuyên mục đã tồn tại hay chưa. 

//var check = await blogRepo.IsCategorySlugExistedAsync("git-lab-2");

//Console.WriteLine(check);

// j. Lấy và phân trang danh sách chuyên mục, kết quả trả về kiểu 
// <CategoryItem>.

//var paringParams = new PagingParams()
//{
//    PageNumber = 1,
//    PageSize = 5,
//    SortColumn = "PostCount",
//    SortOrder = "DESC"
//};

//var categories = await blogRepo.GetPagedCategoriesAsync(paringParams);

//Console.WriteLine("{0, -40}{1, -50}{2, 10}",
//    "ID", "Name", "Count");

//foreach (var category in categories)
//{
//    Console.WriteLine("{0, -40}{1, -50}{2, 10}",
//        category.Id, category.Name, category.PostCount);
//}


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