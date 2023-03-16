using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IBlogRepository _blogRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<CategoryEditModel> _categoryValidator;

        public CategoriesController(IBlogRepository blogRepo, IMapper mapper)
        {
            _blogRepo = blogRepo;
            _mapper = mapper;
            _categoryValidator = new CategoryValidator(_blogRepo);
        }

        public async Task<IActionResult> Index(
            CategoryFilterModel filterModel,
            PagingParams newPaging,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            IPagingParams paging = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortOrder = "DESC",
                SortColumn = "PostCount"
            };

            if (newPaging.PageNumber != 0 && newPaging.PageSize != 0)
            {
                paging.PageSize = newPaging.PageSize;
                paging.PageNumber = newPaging.PageNumber;
            }
            var categoryQuery = _mapper.Map<CategoryQuery>(filterModel);

            var categories = await _blogRepo.GetPagedCategoriesAsync(categoryQuery, paging);

            ViewBag.CategoryFilter  = filterModel;
            return View(categories);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = id != Guid.Empty
                ? await _blogRepo.GetCategoryByIdAsync(id)
                : null;

            var model = category == null
                ? new CategoryEditModel()
                : _mapper.Map<CategoryEditModel>(category);
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditModel model)
        {
            var validationResult = await this._categoryValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return View(model);
            }

            var category = model.Id != Guid.Empty
                ? await _blogRepo.GetCategoryByIdAsync(model.Id)
                : null;

            if (category == null)
            {
                category = _mapper.Map<Category>(model);

                category.Id = Guid.NewGuid();
            }
            else
            {
                _mapper.Map(model, category);
            }

            await _blogRepo.AddOrUpdateCategoryAsync(category);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(
            Guid id,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 3)
        {
            await _blogRepo.DeleteCategoryByIdAsync(id);
            IPagingParams pageParams = new PagingParams()
            {
                PageSize = pageSize,
                PageNumber = pageNumber,
            };
            return RedirectToAction("Index", pageParams);
        }
    }
}
