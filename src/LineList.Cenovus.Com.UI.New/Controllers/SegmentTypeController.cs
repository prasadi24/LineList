using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.SegmentType;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class SegmentTypeController : Controller
    {
        private readonly ISegmentTypeService _segmentTypeService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public SegmentTypeController(ISegmentTypeService segmentTypeService, IMapper mapper, CurrentUser currentUser)
        {
            _segmentTypeService = segmentTypeService ?? throw new ArgumentNullException(nameof(segmentTypeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var segmentTypes = await _segmentTypeService.GetAll();
            var segmentTypeDtos = _mapper.Map<IEnumerable<SegmentTypeResultDto>>(segmentTypes);
            return View(segmentTypeDtos);
        }

        [HttpGet]
        public async Task<JsonResult> SegmentTypeFeed()
        {
            var segmentTypes = await _segmentTypeService.GetAll();
            var segmentTypeDtos = _mapper.Map<IEnumerable<SegmentTypeResultDto>>(segmentTypes);
            return Json(new { data = segmentTypeDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SegmentTypeAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(SegmentTypeAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var segmentType = _mapper.Map<SegmentType>(model);
            var newSegmentType= await _segmentTypeService.Add(segmentType);

            if (newSegmentType == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var segmentType = await _segmentTypeService.GetById(id);
            if (segmentType == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_segmentTypeService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Segment", "Segment Type", segmentType.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var segmentTypeDto = _mapper.Map<SegmentTypeEditDto>(segmentType);
            return PartialView("_Update", segmentTypeDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(SegmentTypeEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var segmentType = _mapper.Map<SegmentType>(model);
            await _segmentTypeService.Update(segmentType);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var segmentType = await _segmentTypeService.GetById(id);
            if (segmentType == null)
                return Json(new { success = false, ErrorMessage = "SegmentType not found" });

            await _segmentTypeService.Remove(segmentType);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentSegmentType = await _segmentTypeService.GetById(request.Id);

            if (currentSegmentType == null)
                return Json(new { success = false, ErrorMessage = "SegmentType not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the SegmentType to swap with (higher for move down, lower for move up)
            var swapSegmentType = (await _segmentTypeService.GetAll())
                .Where(s => isMoveUp ? s.SortOrder < currentSegmentType.SortOrder : s.SortOrder > currentSegmentType.SortOrder)
                .OrderBy(s => isMoveUp ? s.SortOrder * -1 : s.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapSegmentType == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No SegmentType to move up." : "No SegmentType to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentSegmentType.SortOrder;

            currentSegmentType.SortOrder = swapSegmentType.SortOrder;

            swapSegmentType.SortOrder = tempSortOrder;

            // Update both records
            await _segmentTypeService.Update(currentSegmentType);

            await _segmentTypeService.Update(swapSegmentType);

            return Json(new { success = true });
        }
    }
}