using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ISubscriberRepository _subRepo;

        public NewsletterController(ISubscriberRepository subRepo)
        {
            _subRepo = subRepo;
        }

        [HttpGet]
        public IActionResult Subscribe()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {

            var subSuccess = await _subRepo.SubscribeAsync(email);
            var sub = await _subRepo.GetSubscriberByEmail(email);

            ViewData["subSuccess"] = subSuccess;
            return View(sub);
        }

        [HttpGet]
        public IActionResult UnSubscribe()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UnSubscribe(string email, string reason)
        {
            var subSuccess = await _subRepo.UnSubscribeAsync(email, reason);
            var sub = await _subRepo.GetSubscriberByEmail(email);

            ViewData["subSuccess"] = subSuccess;
            return View(sub);
        }

    }
}
