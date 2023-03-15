using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Media;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class PostsController : Controller
    {
        private readonly ILogger<PostsController> _logger;
        private readonly IBlogRepository _blogRepo;
        private readonly IMapper _mapper;
        private readonly IMediaManager _media;
        private readonly IValidator<PostEditModel> _postValidator;

        public PostsController(ILogger<PostsController> logger, IBlogRepository blogRepo, IMapper mapper, IMediaManager media)
        {
            _logger = logger;
            _blogRepo = blogRepo;
            _mapper = mapper;
            _media = media;
            _postValidator = new PostValidator(_blogRepo);
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

        public async Task<IActionResult> Index(
            PostFilterModel model,
            PagingParams pageParams,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            _logger.LogInformation("Tạo điều kiện truy vấn");

            var postQuery = _mapper.Map<PostQuery>(model);

            _logger.LogInformation("Lấy danh sách bài viết từ CSDL");

            if (pageParams.PageNumber != 0 || pageParams.PageSize != 0)
            {
                pageNumber = pageParams.PageNumber;
                pageSize = pageParams.PageSize;
            }

            ViewBag.PostList = await _blogRepo.GetPagedPostsQueryAsync(postQuery, pageNumber, pageSize);

            _logger.LogInformation("Chuẩn bị dữ liệu cho ViewModel");

            await PopulatePostFilterModelAsync(model);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var post = id != Guid.Empty
                ? await _blogRepo.GetPostByIdAsync(id, true)
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
            var validationResult = await this._postValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }

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

            if (model.ImageFile?.Length > 0)
            {
                var newImagePath = await _media.SaveFileAsync(
                    model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);
                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    await _media.DeleteFileAsync(post.ImageUrl);
                    post.ImageUrl = newImagePath;
                }
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

        
        public async Task<IActionResult> TogglePublicStatus(
            Guid id, 
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            await _blogRepo.TogglePublicStatusPostAsync(id);
            IPagingParams pageParams = new PagingParams()
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
            };
            return RedirectToAction("Index", pageParams);
        }

        public async Task<IActionResult> DeletePost(
            Guid id,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            await _blogRepo.DeletePostByIdAsync(id);
            IPagingParams pageParams = new PagingParams()
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
            };
            return RedirectToAction("Index", pageParams);
        }
    }
}
