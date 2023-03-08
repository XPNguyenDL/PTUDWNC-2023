using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Services.Blogs;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
            [FromQuery(Name = "ps")] int pageSize = 3)
        {

            var postQuery = new PostQuery()
            {
                Keyword = keyword,
            };

            var postsList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, pageNumber, pageSize);

            
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }

        public async Task<IActionResult> Tag(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            var postQuery = new PostQuery()
            {
                TagSlug = slug,
            };

            var postsList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }

        public async Task<IActionResult> Author(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            var postQuery = new PostQuery()
            {
                AuthorSlug = slug,
            };

            var postsList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }

        public async Task<IActionResult> Category(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            var postQuery = new PostQuery()
            {
                CategorySlug = slug,
            };

            var postsList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, pageNumber, pageSize);
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }

        public async Task<IActionResult> Post(
            string slug,
            int year,
            int month,
            int day)
        {
          
            var post = await _blogRepo.GetPostAsync(year, month, day, slug);
            await _blogRepo.IncreaseViewCountAsync(post.Id);
            return View(post);
        }

        public async Task<IActionResult> Archives(int year, int month,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            var postQuery = new PostQuery()
            {
                Month = month,
                Year = year,
            };

            var postsList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, pageNumber, pageSize);
            ViewBag.Date = new DateTime(year, month, 1);
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
