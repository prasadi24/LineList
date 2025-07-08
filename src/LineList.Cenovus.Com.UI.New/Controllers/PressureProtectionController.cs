using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.PressureProtection;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class PressureProtectionController : Controller
    {
        private readonly IPressureProtectionService _pressureProtectionService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public PressureProtectionController(IPressureProtectionService pressureProtectionService, IMapper mapper, CurrentUser currentUser)
        {
            _pressureProtectionService = pressureProtectionService ?? throw new ArgumentNullException(nameof(pressureProtectionService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var pressureProtections = await _pressureProtectionService.GetAll();
            var pressureProtectionDtos = _mapper.Map<IEnumerable<PressureProtectionResultDto>>(pressureProtections);
            return View(pressureProtectionDtos);
        }

        [HttpGet]
        public async Task<JsonResult> PressureProtectionFeed()
        {
            var pressureProtections = await _pressureProtectionService.GetAll();
            var pressureProtectionDtos = _mapper.Map<IEnumerable<PressureProtectionResultDto>>(pressureProtections);
            return Json(new { data = pressureProtectionDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PressureProtectionAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(PressureProtectionAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var pressureProtection = _mapper.Map<PressureProtection>(model);
            var newPressureProtection = await _pressureProtectionService.Add(pressureProtection);

            if (newPressureProtection == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }


            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var pressureProtection = await _pressureProtectionService.GetById(id);
            if (pressureProtection == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_pressureProtectionService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Operating Mode", "Pressure Protection", pressureProtection.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var pressureProtectionDto = _mapper.Map<PressureProtectionEditDto>(pressureProtection);
            return PartialView("_Update", pressureProtectionDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(PressureProtectionEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var pressureProtection = _mapper.Map<PressureProtection>(model);
            await _pressureProtectionService.Update(pressureProtection);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var pressureProtection = await _pressureProtectionService.GetById(id);
            if (pressureProtection == null)
                return Json(new { success = false, ErrorMessage = "Pressure Protection not found" });

            string message = "";
            if (_pressureProtectionService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Operating Mode", "Pressure Protection", pressureProtection.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                return Json(new { success = false, ErrorMessage = message });
            }

            await _pressureProtectionService.Remove(pressureProtection);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentPressureProtection = await _pressureProtectionService.GetById(request.Id);

            if (currentPressureProtection == null)
                return Json(new { success = false, ErrorMessage = "PressureProtection not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the PressureProtection to swap with (higher for move down, lower for move up)
            var swapPressureProtection = (await _pressureProtectionService.GetAll())
                .Where(pp => isMoveUp ? pp.SortOrder < currentPressureProtection.SortOrder : pp.SortOrder > currentPressureProtection.SortOrder)
                .OrderBy(pp => isMoveUp ? pp.SortOrder * -1 : pp.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapPressureProtection == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No PressureProtection to move up." : "No PressureProtection to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentPressureProtection.SortOrder;

            currentPressureProtection.SortOrder = swapPressureProtection.SortOrder;

            swapPressureProtection.SortOrder = tempSortOrder;

            // Update both records
            await _pressureProtectionService.Update(currentPressureProtection);

            await _pressureProtectionService.Update(swapPressureProtection);

            return Json(new { success = true });
        }
    }
}