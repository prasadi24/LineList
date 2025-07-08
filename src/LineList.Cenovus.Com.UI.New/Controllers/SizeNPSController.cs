using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.SizeNps;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class SizeNPSController : Controller
    {
        private readonly ISizeNpsService _sizeNPSService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        public SizeNPSController(ISizeNpsService sizeNPSService, IMapper mapper, CurrentUser currentUser)
        {
            _sizeNPSService = sizeNPSService ?? throw new ArgumentNullException(nameof(sizeNPSService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var sizeNps = await _sizeNPSService.GetAll();
            var sizeNpsDtos = _mapper.Map<IEnumerable<SizeNpsResultDto>>(sizeNps);
            return View(sizeNpsDtos);
        }

        [HttpGet]
        public async Task<JsonResult> SizeNPSFeed()
        {
            var sizeNps = await _sizeNPSService.GetAll();
            var sizeNpsDtos = _mapper.Map<IEnumerable<SizeNpsResultDto>>(sizeNps);
            return Json(new { data = sizeNpsDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new SizeNpsAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(SizeNpsAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var sizeNps = _mapper.Map<SizeNps>(model);
            var newSizeNPS = await _sizeNPSService.Add(sizeNps);

            if (newSizeNPS == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var sizeNps = await _sizeNPSService.GetById(id);
            if (sizeNps == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_sizeNPSService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete:\r\n\r\n{0}: {1} is currently referenced by an existing Schedule Default, Line Revision", "Size NPS", sizeNps.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var sizeNpsDto = _mapper.Map<SizeNpsEditDto>(sizeNps);
            return PartialView("_Update", sizeNpsDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(SizeNpsEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var sizeNps = _mapper.Map<SizeNps>(model);
            var updatedSizeNPS = await _sizeNPSService.Update(sizeNps);
            if (updatedSizeNPS == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }


            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var sizeNps = await _sizeNPSService.GetById(id);
            if (sizeNps == null)
                return Json(new { success = false, ErrorMessage = "SizeNPS not found" });

            await _sizeNPSService.Remove(sizeNps);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentSizeNPS = await _sizeNPSService.GetById(request.Id);
            if (currentSizeNPS == null)
                return Json(new { success = false, ErrorMessage = "SizeNPS not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the SizeNPS to swap with (higher for move down, lower for move up)
            var swapSizeNPS = (await _sizeNPSService.GetAll())
                .Where(s => isMoveUp ? s.SortOrder < currentSizeNPS.SortOrder : s.SortOrder > currentSizeNPS.SortOrder)
                .OrderBy(s => isMoveUp ? s.SortOrder * -1 : s.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapSizeNPS == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No SizeNPS to move up." : "No SizeNPS to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentSizeNPS.SortOrder;
            currentSizeNPS.SortOrder = swapSizeNPS.SortOrder;
            swapSizeNPS.SortOrder = tempSortOrder;

            // Update both records
            await _sizeNPSService.Update(currentSizeNPS);
            await _sizeNPSService.Update(swapSizeNPS);

            return Json(new { success = true });
        }
    }
}