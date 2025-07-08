using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Area;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class AreaController : Controller
    {
        private readonly IAreaService _areaService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        private readonly ILocationService _locationService;
        private readonly ISpecificationService _specificationService;

        public AreaController(IAreaService areaService, ILocationService locationService, ISpecificationService specificationService, IMapper mapper, CurrentUser currentUser)
        {
            _areaService = areaService;
            _mapper = mapper;
            _locationService = locationService;
            _specificationService = specificationService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var areas = await _areaService.GetAll();
            var areaDtos = _mapper.Map<IEnumerable<AreaResultDto>>(areas);
            return View(areaDtos);
        }

        [HttpGet]
        public async Task<JsonResult> AreaFeed()
        {
            var areas = await _areaService.GetAll();
            var areaDtos = _mapper.Map<IEnumerable<AreaResultDto>>(areas);
            return Json(new { data = areaDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new AreaAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            ViewBag.Locations = await _locationService.GetAll();
            ViewBag.Specifications = await _specificationService.GetAll();
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(AreaAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var area = _mapper.Map<Area>(model);
            var newArea = await _areaService.Add(area);

            if (newArea == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var area = await _areaService.GetById(id);
            if (area == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_areaService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision or Line List Revision", "Area :", area.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var areaDto = _mapper.Map<AreaEditDto>(area);

            ViewBag.Locations = await _locationService.GetAll();
            ViewBag.Specifications = await _specificationService.GetAll();

            return PartialView("_Update", areaDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(AreaEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var area = _mapper.Map<Area>(model);
            var updatedArea = await _areaService.Update(area);

            if (updatedArea == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var area = await _areaService.GetById(id);
            if (area == null)
                return Json(new { success = false, ErrorMessage = "Area not found" });

            await _areaService.Remove(area);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentArea = await _areaService.GetById(request.Id);
            if (currentArea == null)
                return Json(new { success = false, ErrorMessage = "Area not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the area to swap with (higher for move down, lower for move up)
            var swapArea = (await _areaService.GetAll())
                .Where(a => isMoveUp ? a.SortOrder < currentArea.SortOrder : a.SortOrder > currentArea.SortOrder)
                .OrderBy(a => isMoveUp ? a.SortOrder * -1 : a.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapArea == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No area to move up." : "No area to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentArea.SortOrder;
            currentArea.SortOrder = swapArea.SortOrder;
            swapArea.SortOrder = tempSortOrder;

            // Update both records
            await _areaService.Update(currentArea);
            await _areaService.Update(swapArea);

            return Json(new { success = true });
        }
    }
}