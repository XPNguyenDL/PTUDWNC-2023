using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using TatBlog.Core.Collections;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Models;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace TatBlog.WebApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogRepo;
        private readonly ICommentRepository _cmtRepo;
        private IConfiguration _configuration;

        public BlogController(IBlogRepository blogRepo, ICommentRepository cmtRepo, IConfiguration configuration)
        {
            _blogRepo = blogRepo;
            _cmtRepo = cmtRepo;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(
            [FromQuery(Name = "k")] string keyword = "",
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
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

        public async Task<IActionResult> Tag(
            string slug,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            var postQuery = new PostQuery()
            {
                TagSlug = slug,
                Published = true
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
                Published = true
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
                Published = true
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
            var cmtList = await _cmtRepo.GetCommentsByPost(post.Id);

            ViewData["Comments"] = cmtList;
            return View(post);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Guid postId, string userName, string content)
        {
            try
            {
                var newCmt = new Comment()
                {
                    Id = Guid.NewGuid(),
                    PostId = postId,
                    Active = true,
                    CommentStatus = CommentStatus.Violate,
                    Content = content,
                    UserComment = userName,
                    PostTime = DateTime.Now
                };

                var cmtSuccess = await _cmtRepo.AddOrUpdateCommentAsync(newCmt);
                var cmtList = await _cmtRepo.GetCommentsByPost(postId);

                ViewData["Comments"] = cmtList;
                ViewBag.CmtSuccess = cmtSuccess;


                var post = await _blogRepo.GetPostByIdAsync(postId);
                return View(post);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Error", e.Message);
                return BadRequest(ModelState);
            }
        }

        public async Task<IActionResult> Archives(int year, int month,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            var postQuery = new PostQuery()
            {
                Month = month,
                Year = year,
                Published = true
            };

            var postsList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, pageNumber, pageSize);
            ViewBag.Date = new DateTime(year, month, 1);
            ViewBag.PostQuery = postQuery;
            return View(postsList);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Contact(string email, string subject, string body)
        {
            try
            {
                var content = body.Replace("\n", "<br>");

                var emailModel = new EmailModel()
                {
                    Subject = $"Phản hồi từ {email}",
                    Body = $"{subject}:<br> {content}"
                };

                SendEmail(emailModel);

                ViewBag.Success = true;
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                return BadRequest(ModelState);
            }

        }

        public IActionResult Rss()
        {
            return Content("Nội dung sẽ được cập nhật");
        }

        private void SendEmail(EmailModel emailModel)
        {
            var host = this._configuration.GetValue<string>("Smtp:Server");
            var port = this._configuration.GetValue<int>("Smtp:Port");
            var fromAddress = this._configuration.GetValue<string>("Smtp:FromAddress");
            var adminAddress = this._configuration.GetValue<string>("Smtp:AdminEmail");
            var userName = this._configuration.GetValue<string>("Smtp:UserName");
            var password = this._configuration.GetValue<string>("Smtp:Password");


            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(fromAddress);
                mail.To.Add(adminAddress);
                mail.Subject = emailModel.Subject;
                mail.Body = emailModel.Body;
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
