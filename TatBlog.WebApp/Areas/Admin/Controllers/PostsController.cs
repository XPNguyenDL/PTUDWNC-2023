using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private readonly IBlogRepository _blogRepo;

        public PostsController(IBlogRepository blogRepo)
        {
            _blogRepo = blogRepo;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
