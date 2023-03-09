using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.SignalR;

namespace TatBlog.WebApp.Controllers
{
    
    public class NewsletterController : Controller
    {
        private readonly ISubscriberRepository _subRepo;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private  IConfiguration _configuration;


        public NewsletterController(ISubscriberRepository subRepo, IWebHostEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _subRepo = subRepo;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
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

                if (subSuccess)
                {
                    SendEmail(sub.Email);
                }

                ViewBag.SubSuccess = subSuccess;
                return View(sub);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(ModelState);
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

            ViewBag.SubSuccess = subSuccess;
            return View(sub);
        }

        private void SendEmail(string email)
        {
            var host = this._configuration.GetValue<string>("Smtp:Server");
            var port = this._configuration.GetValue<int>("Smtp:Port");
            var fromAddress = this._configuration.GetValue<string>("Smtp:FromAddress");
            var userName = this._configuration.GetValue<string>("Smtp:UserName");
            var password = this._configuration.GetValue<string>("Smtp:Password");

            string wwwrootPath = _hostingEnvironment.WebRootPath;
            string filePath = Path.Combine(wwwrootPath, "templates/emails/EmailSubscribe.html");
            string fileContents = System.IO.File.ReadAllText(filePath);

            fileContents = fileContents.Replace("{link}", "https://localhost:7245/Newsletter/UnSubscribe");

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(fromAddress);
                mail.To.Add(email);
                mail.Subject = $"Xác nhận đăng ký nhận thông báo.";
                mail.Body = fileContents;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(host, port))
                {
                    smtp.Credentials = new NetworkCredential(userName,
                        password);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
