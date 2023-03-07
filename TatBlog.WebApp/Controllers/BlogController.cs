using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepo;

        public BlogController(IBlogRepository blogRepo)
        {
            _blogRepo = blogRepo;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = null,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {

            var postQuery = new PostQuery()
            {
                Keyword = keyword,
                Published = true
            };

            var postsList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, pageNumber, pageSize);



            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Rss()
        {
            return Content("Nội dung sẽ được cập nhật");
        }
    }
}
