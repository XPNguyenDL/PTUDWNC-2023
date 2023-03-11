using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TatBlog.Core.Collections;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private readonly IBlogRepository _blogRepo;
        private readonly IMapper _mapper;

        public PostsController(IBlogRepository blogRepo, IMapper mapper)
        {
            _blogRepo = blogRepo;
            _mapper = mapper;
        }

        private async Task PopulatePostFilterModelAsync(PostFilterModel model)
        {
            var authors = await _blogRepo.GetAuthorAsync();
            var categories = await _blogRepo.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }

        private async Task PopulatePostEditModelAsync(PostEditModel model)
        {
            var authors = await _blogRepo.GetAuthorAsync();
            var categories = await _blogRepo.GetCategoriesAsync();

            model.AuthorList = authors.Select(a => new SelectListItem()
            {
                Text = a.FullName,
                Value = a.Id.ToString()
            });

            model.CategoryList = categories.Select(s => new SelectListItem()
            {
                Text = s.Name,
                Value = s.Id.ToString()
            });
        }

        public async Task<IActionResult> Index(PostFilterModel model)
        {
            var postQuery = _mapper.Map<PostQuery>(model);

            ViewBag.PostList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, 1, 10);

            await PopulatePostFilterModelAsync(model);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = id != Guid.Empty
                ? await _blogRepo.GetPostByIdAsync(id)
                : null;

            var model = post == null
                ? new PostEditModel()
                : _mapper.Map<PostEditModel>(post);

            await PopulatePostEditModelAsync(model);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostEditModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
                await PopulatePostEditModelAsync(model);
                return View(model);
            }

            var post = model.Id != Guid.Empty
                ? await _blogRepo.GetPostByIdAsync(model.Id)
                : null;

            if (post == null)
            {
                post = _mapper.Map<Post>(model);

                post.Id = Guid.NewGuid();
                post.PostedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, post);

                post.Category = null;
                post.ModifiedDate = DateTime.Now;
            }

            await _blogRepo.AddOrUpdatePostAsync(post, model.GetSelectTags());

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> VerifyPostSlug(Guid id, string urlSlug)
        {
            var slugExisted = await _blogRepo.IsPostSlugExistedAsync(id, urlSlug);

            return slugExisted
                ? Json($"Slug '{urlSlug}' đã được sử dụng")
                : Json(true);
        }
    }
}
