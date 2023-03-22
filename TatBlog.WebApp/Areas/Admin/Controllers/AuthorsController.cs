using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Media;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _authorRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<AuthorEditModel> _authorValidator;
        private readonly IMediaManager _media;

        public AuthorsController(IAuthorRepository authorRepo, IMapper mapper, IMediaManager media)
        {
            _authorRepo = authorRepo;
            _mapper = mapper;
            _media = media;
            _authorValidator = new AuthorValidator(authorRepo);
        }

        public async Task<IActionResult> Index(
            AuthorFilterModel filterModel,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            IPagingParams paging = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = "FullName"
            };

            var authorQuery = _mapper.Map<AuthorQuery>(filterModel);

            var authorPage = await _authorRepo.GetPagedAuthorsAsync(authorQuery, paging);

            ViewBag.AuthorFilter = filterModel;

            return View(authorPage);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var author = id != Guid.Empty
                ? await _authorRepo.GetAuthorByIdAsync(id)
                : null;

            var model = author == null
                ? new AuthorEditModel()
                : _mapper.Map<AuthorEditModel>(author);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AuthorEditModel model)
        {
            var validationResult = await this._authorValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return View(model);
            }

            var author = model.Id != Guid.Empty
                ? await _authorRepo.GetAuthorByIdAsync(model.Id)
                : null;

            if (author == null)
            {
                author = _mapper.Map<Author>(model);

                author.Id = Guid.NewGuid();
                author.JoinedDate = DateTime.Now;
            }
            else
            {
                _mapper.Map(model, author);
            }

            if (model.ImageFile?.Length > 0)
            {
                var newImagePath = await _media.SaveFileAsync(
                    model.ImageFile.OpenReadStream(),
                    model.ImageFile.FileName,
                    model.ImageFile.ContentType);
                if (!string.IsNullOrWhiteSpace(newImagePath))
                {
                    await _media.DeleteFileAsync(author.ImageUrl);
                    author.ImageUrl = newImagePath;
                }
            }

            await _authorRepo.AddOrUpdateAuthorAsync(author);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(
            Guid id,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            await _authorRepo.DeleteAuthorByIdAsync(id);
            return RedirectToAction("Index", "Authors", new { pageSize = pageSize, pageNumber = pageNumber });
        }
    }
}
