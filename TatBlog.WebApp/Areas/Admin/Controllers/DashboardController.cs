using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Services.Blogs;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ICommentRepository _cmtRepo;
        private readonly IBlogRepository _blogRepo;
        private readonly IAuthorRepository _authorRepo;
        private readonly ISubscriberRepository _subscriberRepo;
        private readonly IMapper _mapper;

        public DashboardController(ICommentRepository cmtRepo, IAuthorRepository authorRepo, IBlogRepository blogRepo, ISubscriberRepository subscriberRepo, IMapper mapper)
        {
            _cmtRepo = cmtRepo;
            _mapper = mapper;
            _authorRepo = authorRepo;
            _blogRepo = blogRepo;
            _subscriberRepo = subscriberRepo;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.PostCount = await _blogRepo.CountPostAsync();
            ViewBag.PostUnPublicCount = await _blogRepo.CountPostUnPublicAsync();
            ViewBag.CmtCount = await _cmtRepo.CountCommentAsync();
            ViewBag.AuthorCount = await _authorRepo.CountAuthorAsync();
            ViewBag.SubDailyCount = await _subscriberRepo.CountSubscriberByDayAsync();
            ViewBag.SubCount = await _subscriberRepo.CountSubscriberAsync();
            ViewBag.CmtNotVerifyCount = await _cmtRepo.CountCommentNotVerifyAsync();
            return View();
        }
    }
}
