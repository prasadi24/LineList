using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.TracingType;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class TracingTypeController : Controller
    {
        private readonly ITracingTypeService _tracingTypeService;
        private readonly IMapper _mapper;
        private readonly ISpecificationService _specificationService;
        private readonly CurrentUser _currentUser;

        public TracingTypeController(ITracingTypeService tracingTypeService, IMapper mapper, ISpecificationService specificationService, CurrentUser currentUser)
        {
            _tracingTypeService = tracingTypeService ?? throw new ArgumentNullException(nameof(tracingTypeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _specificationService = specificationService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var tracingTypes = await _tracingTypeService.GetAll();
            var tracingTypeDtos = _mapper.Map<IEnumerable<TracingTypeResultDto>>(tracingTypes);
            return View(tracingTypeDtos);
        }

        [HttpGet]
        public async Task<JsonResult> TracingTypeFeed()
        {
            var tracingTypes = await _tracingTypeService.GetAll();
            var tracingTypeDtos = _mapper.Map<IEnumerable<TracingTypeResultDto>>(tracingTypes);
            return Json(new { data = tracingTypeDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new TracingTypeAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };

            ViewBag.Specifications = await _specificationService.GetAll();
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(TracingTypeAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var tracingType = _mapper.Map<TracingType>(model);
            var newTracingType = await _tracingTypeService.Add(tracingType);
            if (newTracingType == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var tracingType = await _tracingTypeService.GetById(id);
            if (tracingType == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_tracingTypeService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line revision Segment, Insulation Default", "Tracing Type", tracingType.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var tracingTypeDto = _mapper.Map<TracingTypeEditDto>(tracingType);

            ViewBag.Specifications = await _specificationService.GetAll();
            return PartialView("_Update", tracingTypeDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(TracingTypeEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var tracingType = _mapper.Map<TracingType>(model);
            var updatedTracingType = await _tracingTypeService.Update(tracingType);

            if (updatedTracingType == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var tracingType = await _tracingTypeService.GetById(id);
            if (tracingType == null)
                return Json(new { success = false, ErrorMessage = "TracingType not found" });

            await _tracingTypeService.Remove(tracingType);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentTracingType = await _tracingTypeService.GetById(request.Id);
            if (currentTracingType == null)
                return Json(new { success = false, ErrorMessage = "TracingType not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the TracingType to swap with (higher for move down, lower for move up)
            var swapTracingType = (await _tracingTypeService.GetAll())
                .Where(tt => isMoveUp ? tt.SortOrder < currentTracingType.SortOrder : tt.SortOrder > currentTracingType.SortOrder)
                .OrderBy(tt => isMoveUp ? tt.SortOrder * -1 : tt.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapTracingType == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No TracingType to move up." : "No TracingType to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentTracingType.SortOrder;
            currentTracingType.SortOrder = swapTracingType.SortOrder;
            swapTracingType.SortOrder = tempSortOrder;

            // Update both records
            await _tracingTypeService.Update(currentTracingType);
            await _tracingTypeService.Update(swapTracingType);

            return Json(new { success = true });
        }
    }
}