using FluentValidation;
using FluentValidation.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Net.NetworkInformation;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class TagsController : Controller
    {

        private readonly IBlogRepository _blogRepo;
        private readonly IMapper _mapper;
        private readonly IValidator<TagEditModel> _tagValidator;

        public TagsController(IBlogRepository blogRepo, IMapper mapper)
        {
            _blogRepo = blogRepo;
            _mapper = mapper;
            _tagValidator = new TagValidator(_blogRepo);
        }

        public async Task<IActionResult> Index(
            string keyword,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            IPagingParams pager = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = "PostCount",
                SortOrder = "DESC"
            };
            var tag = await _blogRepo.GetPagedTagsAsync(keyword, pager);
            
            return View(tag);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = id != Guid.Empty
                ? await _blogRepo.GetTagByIdAsync(id)
                : null;

            var model = tag == null
                ? new TagEditModel()
                : _mapper.Map<TagEditModel>(tag);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TagEditModel model)
        {
            var validationResult = await this._tagValidator.ValidateAsync(model);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
            }

            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return View(model);
            }

            var tag = model.Id != Guid.Empty
                ? await _blogRepo.GetTagByIdAsync(model.Id)
                : null;

            if (tag == null)
            {
                tag = _mapper.Map<Tag>(model);

                tag.Id = Guid.NewGuid();
            }
            else
            {
                _mapper.Map(model, tag);
            }

            await _blogRepo.AddOrUpdateTagAsync(tag);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(
            Guid id,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 10)
        {
            await _blogRepo.DeleteTagByIdAsync(id);
            return RedirectToAction("Index", "Tags", new { pageSize = pageSize, pageNumber = pageNumber });
        }
    }
}
