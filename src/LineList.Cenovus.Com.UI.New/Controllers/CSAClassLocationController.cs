using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.CsaClassLocation;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class CsaClassLocationController : Controller
    {
        private readonly ICsaClassLocationService _csaClassLocationService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public CsaClassLocationController(ICsaClassLocationService csaClassLocationService, IMapper mapper, CurrentUser currentUser)
        {
            _csaClassLocationService = csaClassLocationService ?? throw new ArgumentNullException(nameof(csaClassLocationService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var locations = await _csaClassLocationService.GetAll();
            var locationDtos = _mapper.Map<IEnumerable<CsaClassLocationResultDto>>(locations);
            return View(locationDtos);
        }

        [HttpGet]
        public async Task<JsonResult> CsaClassLocationFeed()
        {
            var locations = await _csaClassLocationService.GetAll();
            var locationDtos = _mapper.Map<IEnumerable<CsaClassLocationResultDto>>(locations);
            return Json(new { data = locationDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CsaClassLocationAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(CsaClassLocationAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var location = _mapper.Map<CsaClassLocation>(model);
            var newLocation = await _csaClassLocationService.Add(location);

            if (newLocation == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var location = await _csaClassLocationService.GetById(id);
            if (location == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_csaClassLocationService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Operating Mode", "CSA Class Location", location.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var locationDto = _mapper.Map<CsaClassLocationEditDto>(location);
            return PartialView("_Update", locationDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(CsaClassLocationEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var location = _mapper.Map<CsaClassLocation>(model);
            await _csaClassLocationService.Update(location);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var location = await _csaClassLocationService.GetById(id);
            if (location == null)
                return Json(new { success = false, ErrorMessage = "CsaClassLocation not found" });
                       
            await _csaClassLocationService.Remove(location);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentCsaClassLocation = await _csaClassLocationService.GetById(request.Id);

            if (currentCsaClassLocation == null)
                return Json(new { success = false, ErrorMessage = "CsaClassLocation not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the CsaClassLocation to swap with (higher for move down, lower for move up)
            var swapCsaClassLocation = (await _csaClassLocationService.GetAll())
                .Where(c => isMoveUp ? c.SortOrder < currentCsaClassLocation.SortOrder : c.SortOrder > currentCsaClassLocation.SortOrder)
                .OrderBy(c => isMoveUp ? c.SortOrder * -1 : c.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapCsaClassLocation == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No CsaClassLocation to move up." : "No CsaClassLocation to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentCsaClassLocation.SortOrder;

            currentCsaClassLocation.SortOrder = swapCsaClassLocation.SortOrder;

            swapCsaClassLocation.SortOrder = tempSortOrder;

            // Update both records
            await _csaClassLocationService.Update(currentCsaClassLocation);

            await _csaClassLocationService.Update(swapCsaClassLocation);

            return Json(new { success = true });
        }
    }
}