
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentRepository _cmtRepo;
        private readonly IMapper _mapper;

        public CommentsController(ICommentRepository cmtRepo, IMapper mapper)
        {
            _cmtRepo = cmtRepo;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(
            CommentFilterModel filterModel,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            IPagingParams paging = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = "UserComment"
            };

            var cmtQuery = _mapper.Map<CommentQuery>(filterModel);
            var cmtPage = await _cmtRepo.GetPagedCommentAsync(cmtQuery, paging);
            ViewBag.CommentFilterModel = filterModel;
            return View(cmtPage);
        }

        public async Task<IActionResult> Delete(
            Guid id,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            await _cmtRepo.DeleteCommentAsync(id);
            return RedirectToAction("Index", "Comments", new { pageSize = pageSize, pageNumber = pageNumber });
        }

        public async Task<IActionResult> VerifyComment(
            Guid id,
            CommentStatus commentStatus)
        {
            await _cmtRepo.VerifyCommentAsync(id, commentStatus);
            return RedirectToAction("Index");
        }
    }
}
