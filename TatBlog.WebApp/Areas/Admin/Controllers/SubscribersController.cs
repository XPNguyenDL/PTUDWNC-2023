using FluentValidation;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using TatBlog.Core.Collections;
using TatBlog.Core.Contracts;
using TatBlog.Core.Entities;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Areas.Admin.Models;
using TatBlog.WebApp.Validations;

namespace TatBlog.WebApp.Areas.Admin.Controllers
{
    public class SubscribersController : Controller
    {
        private readonly ISubscriberRepository _subRepo;
        private readonly IMapper _mapper;

        public SubscribersController(ISubscriberRepository subRepo, IMapper mapper)
        {
            _subRepo = subRepo;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(
            SubscriberFilterModel filterModel,
            string keyword,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {

            IPagingParams paging = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = "Email"
            };

            var subQuery = _mapper.Map<SubscriberQuery>(filterModel);

            var categories = await _subRepo.SearchSubscribersAsync(subQuery, paging);

            ViewBag.SubscriberFilter = filterModel;
            return View(categories);
        }

        public async Task<IActionResult> Delete(
            Guid id,
            [FromQuery(Name = "p")] int pageNumber = 1,
            [FromQuery(Name = "ps")] int pageSize = 5)
        {
            await _subRepo.DeleteSubscriberAsync(id);
            return RedirectToAction("Index", "Subscribers", new { pageSize = pageSize, pageNumber = pageNumber });
        }

        [HttpGet]
        public async Task<IActionResult> BlockSubscriber(Guid id)
        {
            var sub = id != Guid.Empty
                ? await _subRepo.GetSubscriberByIdAsync(id)
                : null;

            var model = sub == null
                ? new SubscriberEditModel()
                : _mapper.Map<SubscriberEditModel>(sub);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BlockSubscriber(
            SubscriberEditModel subscriber)
        {
            var sub = await _subRepo.GetSubscriberByIdAsync(subscriber.Id);

            await _subRepo.BlockSubscribeAsync(sub.Id, subscriber.Reason, subscriber.Note);
            return RedirectToAction("Index");
        }
    }
}
