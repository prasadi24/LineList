using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.InternalCoatingLiner;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InternalCoatingLinerController : Controller
    {
        private readonly IInternalCoatingLinerService _internalCoatingLinerService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public InternalCoatingLinerController(IInternalCoatingLinerService internalCoatingLinerService, IMapper mapper, CurrentUser currentUser)
        {
            _internalCoatingLinerService = internalCoatingLinerService ?? throw new ArgumentNullException(nameof(internalCoatingLinerService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var internalCoatingLiners = await _internalCoatingLinerService.GetAll();
            var internalCoatingLinerDtos = _mapper.Map<IEnumerable<InternalCoatingLinerResultDto>>(internalCoatingLiners);
            return View(internalCoatingLinerDtos);
        }

        [HttpGet]
        public async Task<JsonResult> InternalCoatingLinerFeed()
        {
            var internalCoatingLiners = await _internalCoatingLinerService.GetAll();
            var internalCoatingLinerDtos = _mapper.Map<IEnumerable<InternalCoatingLinerResultDto>>(internalCoatingLiners);
            return Json(new { data = internalCoatingLinerDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new InternalCoatingLinerAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(InternalCoatingLinerAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var internalCoatingLiner = _mapper.Map<InternalCoatingLiner>(model);
            var newInternalCoatingLiner = await _internalCoatingLinerService.Add(internalCoatingLiner);

            if (newInternalCoatingLiner == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var internalCoatingLiner = await _internalCoatingLinerService.GetById(id);
            if (internalCoatingLiner == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_internalCoatingLinerService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision", "Internal Coating Liner", internalCoatingLiner.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var internalCoatingLinerDto = _mapper.Map<InternalCoatingLinerEditDto>(internalCoatingLiner);
            return PartialView("_Update", internalCoatingLinerDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InternalCoatingLinerEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var internalCoatingLiner = _mapper.Map<InternalCoatingLiner>(model);
            await _internalCoatingLinerService.Update(internalCoatingLiner);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var internalCoatingLiner = await _internalCoatingLinerService.GetById(id);
            if (internalCoatingLiner == null)
                return Json(new { success = false, ErrorMessage = "Internal Coating Liner not found" });
                      
            await _internalCoatingLinerService.Remove(internalCoatingLiner);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentInternalCoatingLiner = await _internalCoatingLinerService.GetById(request.Id);

            if (currentInternalCoatingLiner == null)
                return Json(new { success = false, ErrorMessage = "Internal Coating Liner not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the Internal Coating Liner to swap with (higher for move down, lower for move up)
            var swapInternalCoatingLiner = (await _internalCoatingLinerService.GetAll())
                .Where(i => isMoveUp ? i.SortOrder < currentInternalCoatingLiner.SortOrder : i.SortOrder > currentInternalCoatingLiner.SortOrder)
                .OrderBy(i => isMoveUp ? i.SortOrder * -1 : i.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapInternalCoatingLiner == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No Internal Coating Liner to move up." : "No Internal Coating Liner to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentInternalCoatingLiner.SortOrder;

            currentInternalCoatingLiner.SortOrder = swapInternalCoatingLiner.SortOrder;

            swapInternalCoatingLiner.SortOrder = tempSortOrder;

            // Update both records
            await _internalCoatingLinerService.Update(currentInternalCoatingLiner);

            await _internalCoatingLinerService.Update(swapInternalCoatingLiner);

            return Json(new { success = true });
        }
    }
}