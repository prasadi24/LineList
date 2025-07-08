using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.LocationType;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class LocationTypeController : Controller
    {
        private readonly ILocationTypeService _locationTypeService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public LocationTypeController(ILocationTypeService locationTypeService, IMapper mapper, CurrentUser currentUser)
        {
            _locationTypeService = locationTypeService ?? throw new ArgumentNullException(nameof(locationTypeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locationTypes = await _locationTypeService.GetAll();
            var locationTypeDtos = _mapper.Map<IEnumerable<LocationTypeResultDto>>(locationTypes);
            return View(locationTypeDtos);
        }

        [HttpGet]
        public async Task<JsonResult> LocationTypeFeed()
        {
            var locationTypes = await _locationTypeService.GetAll();
            var locationTypeDtos = _mapper.Map<IEnumerable<LocationTypeResultDto>>(locationTypes);
            return Json(new { data = locationTypeDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new LocationTypeAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(LocationTypeAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var locationType = _mapper.Map<LocationType>(model);
            var newLocationType = await _locationTypeService.Add(locationType);

            if (newLocationType == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var locationType = await _locationTypeService.GetById(id);
            if (locationType == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_locationTypeService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Location", "Location Type", locationType.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var locationTypeDto = _mapper.Map<LocationTypeEditDto>(locationType);
            return PartialView("_Update", locationTypeDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(LocationTypeEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var locationType = _mapper.Map<LocationType>(model);
            var updatedLocationType = await _locationTypeService.Update(locationType);

            if (updatedLocationType == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var locationType = await _locationTypeService.GetById(id);
            if (locationType == null)
                return Json(new { success = false, ErrorMessage = "Location Type not found" });

            await _locationTypeService.Remove(locationType);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentLocationType = await _locationTypeService.GetById(request.Id);
            if (currentLocationType == null)
                return Json(new { success = false, ErrorMessage = "LocationType not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the location type to swap with (higher for move down, lower for move up)
            var swapLocationType = (await _locationTypeService.GetAll())
                .Where(lt => isMoveUp ? lt.SortOrder < currentLocationType.SortOrder : lt.SortOrder > currentLocationType.SortOrder)
                .OrderBy(lt => isMoveUp ? lt.SortOrder * -1 : lt.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapLocationType == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No LocationType to move up." : "No LocationType to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentLocationType.SortOrder;
            currentLocationType.SortOrder = swapLocationType.SortOrder;
            swapLocationType.SortOrder = tempSortOrder;

            // Update both records
            await _locationTypeService.Update(currentLocationType);
            await _locationTypeService.Update(swapLocationType);

            return Json(new { success = true });
        }
    }
}