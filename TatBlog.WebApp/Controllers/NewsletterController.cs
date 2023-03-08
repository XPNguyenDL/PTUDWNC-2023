using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using System.Net.Mail;
using System.Net;

namespace TatBlog.WebApp.Controllers
{
    public class NewsletterController : Controller
    {
        private readonly ISubscriberRepository _subRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public NewsletterController(ISubscriberRepository subRepo, IWebHostEnvironment hostingEnvironment)
        {
            _subRepo = subRepo;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Subscribe()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(string email)
        {
            try
            {
                var subSuccess = await _subRepo.SubscribeAsync(email);
                var sub = await _subRepo.GetSubscriberByEmail(email);

                string wwwrootPath = _hostingEnvironment.WebRootPath;
                string filePath = Path.Combine(wwwrootPath, "templates/emails/EmailSubscribe.html");
                string fileContents = System.IO.File.ReadAllText(filePath);
                
                fileContents = fileContents.Replace("{link}", "https://localhost:7245/Newsletter/UnSubscribe");

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("noreply.email.dluconfession@gmail.com");
                    mail.To.Add(sub.Email);
                    mail.Subject = $"Xác nhận đăng ký nhận thông báo.";
                    mail.Body = fileContents;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("noreply.email.dluconfession@gmail.com",
                            "upngspyyxjhrpjqd");
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                ViewData["subSuccess"] = subSuccess;
                return View(sub);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
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
