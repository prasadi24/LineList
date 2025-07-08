using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Location;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class LocationController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        private readonly IFacilityService _facilityService;
        private readonly ILocationTypeService _locationTypeService;

        public LocationController(ILocationService locationService, IFacilityService facilityService, ILocationTypeService locationTypeService, IMapper mapper, CurrentUser currentUser)
        {
            _locationService = locationService;
            _mapper = mapper;
            _facilityService = facilityService;
            _locationTypeService = locationTypeService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locations = await _locationService.GetAll();
            var locationDtos = _mapper.Map<IEnumerable<LocationResultDto>>(locations);
            return View(locationDtos);
        }

        [HttpGet]
        public async Task<JsonResult> LocationFeed()
        {
            var locations = await _locationService.GetAll();
            var locationDtos = _mapper.Map<IEnumerable<LocationResultDto>>(locations);
            return Json(new { data = locationDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new LocationAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            ViewBag.Facility = await _facilityService.GetAll();
            ViewBag.LocationType = await _locationTypeService.GetAll();
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(LocationAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var location = _mapper.Map<Location>(model);
            var newLocation = await _locationService.Add(location);

            if (newLocation == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var location = await _locationService.GetById(id);
            if (location == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_locationService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line, Line Revision, Line List Revision", "location", location.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var locationDto = _mapper.Map<LocationEditDto>(location);

            ViewBag.Facility = await _facilityService.GetAll();
            ViewBag.LocationType = await _locationTypeService.GetAll();

            return PartialView("_Update", locationDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(LocationEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var location = _mapper.Map<Location>(model);
            var updatedLocation = await _locationService.Update(location);
            if (updatedLocation == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var location = await _locationService.GetById(id);
            if (location == null)
                return Json(new { success = false, ErrorMessage = "Location not found" });

            await _locationService.Remove(location);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentLocation = await _locationService.GetById(request.Id);
            if (currentLocation == null)
                return Json(new { success = false, ErrorMessage = "Location not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the location to swap with (higher for move down, lower for move up)
            var swapLocation = (await _locationService.GetAll())
                .Where(l => isMoveUp ? l.SortOrder < currentLocation.SortOrder : l.SortOrder > currentLocation.SortOrder)
                .OrderBy(l => isMoveUp ? l.SortOrder * -1 : l.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapLocation == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No location to move up." : "No location to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentLocation.SortOrder;
            currentLocation.SortOrder = swapLocation.SortOrder;
            swapLocation.SortOrder = tempSortOrder;

            // Update both records
            await _locationService.Update(currentLocation);
            await _locationService.Update(swapLocation);

            return Json(new { success = true });
        }
    }
}