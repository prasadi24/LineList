using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Facility;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class FacilityController : Controller
    {
        private readonly IFacilityService _facilityService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public FacilityController(IFacilityService facilityService, IMapper mapper, CurrentUser currentUser)
        {
            _facilityService = facilityService ?? throw new ArgumentNullException(nameof(facilityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["IsCenovusAdmin"] = _currentUser.IsCenovusAdmin;
            var facilities = await _facilityService.GetAll();
            var facilityDtos = _mapper.Map<IEnumerable<FacilityResultDto>>(facilities);
            return View(facilityDtos);
        }

        [HttpGet]
        public async Task<JsonResult> FacilityFeed()
        {
            var facilities = await _facilityService.GetAll();
            var facilityDtos = _mapper.Map<IEnumerable<FacilityResultDto>>(facilities);
            return Json(new { data = facilityDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new FacilityAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(FacilityAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var facility = _mapper.Map<Facility>(model);
            var newFacility = await _facilityService.Add(facility);

            if (newFacility == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var facility = await _facilityService.GetById(id);
            if (facility == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_facilityService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Location or Project Group / Expansion Phase Or EP Company Alpha or Notes Configuration", "Facility", facility.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var facilityDto = _mapper.Map<FacilityEditDto>(facility);
            return PartialView("_Update", facilityDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(FacilityEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            var facility = _mapper.Map<Facility>(model);
            var updatedFacility = await _facilityService.Update(facility);

            if (updatedFacility == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var facility = await _facilityService.GetById(id);
            if (facility == null)
                return Json(new { success = false, ErrorMessage = "Facility not found" });

            await _facilityService.Remove(facility);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentFacility = await _facilityService.GetById(request.Id);
            if (currentFacility == null)
                return Json(new { success = false, ErrorMessage = "Facility not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the facility to swap with (higher for move down, lower for move up)
            var swapFacility = (await _facilityService.GetAll())
                .Where(f => isMoveUp ? f.SortOrder < currentFacility.SortOrder : f.SortOrder > currentFacility.SortOrder)
                .OrderBy(f => isMoveUp ? f.SortOrder * -1 : f.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapFacility == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No facility to move up." : "No facility to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentFacility.SortOrder;
            currentFacility.SortOrder = swapFacility.SortOrder;
            swapFacility.SortOrder = tempSortOrder;

            // Update both records
            await _facilityService.Update(currentFacility);
            await _facilityService.Update(swapFacility);

            return Json(new { success = true });
        }

        public async Task<JsonResult> IsNameUnique([FromBody] NameUniqueRequest request)
        {
            if ( string.IsNullOrEmpty(request.Name))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var isAvailbale =  !_facilityService.GetAll().Result.Any(n=>n.Name == request.Name);

            return Json(new { success = isAvailbale });
        }
    }
}