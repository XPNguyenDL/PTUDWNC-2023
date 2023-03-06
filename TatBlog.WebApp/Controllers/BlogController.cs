using Microsoft.AspNetCore.Mvc;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
