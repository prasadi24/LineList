using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.OperatingMode;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class OperatingModeController : Controller
    {
        private readonly IOperatingModeService _operatingModeService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public OperatingModeController(IOperatingModeService operatingModeService, IMapper mapper, CurrentUser currentUser)
        {
            _operatingModeService = operatingModeService ?? throw new ArgumentNullException(nameof(operatingModeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var operatingModes = await _operatingModeService.GetAll();
            var operatingModeDtos = _mapper.Map<IEnumerable<OperatingModeResultDto>>(operatingModes);
            return View(operatingModeDtos);
        }

        [HttpGet]
        public async Task<JsonResult> OperatingModeFeed()
        {
            var operatingModes = await _operatingModeService.GetAll();
            var operatingModeDtos = _mapper.Map<IEnumerable<OperatingModeResultDto>>(operatingModes);
            return Json(new { data = operatingModeDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new OperatingModeAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(OperatingModeAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var operatingMode = _mapper.Map<OperatingMode>(model);
            var newOperatingMode = await _operatingModeService.Add(operatingMode);

            if (newOperatingMode == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var operatingMode = await _operatingModeService.GetById(id);
            if (operatingMode == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_operatingModeService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Operating Mode", "Operating Mode", operatingMode.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var operatingModeDto = _mapper.Map<OperatingModeEditDto>(operatingMode);
            return PartialView("_Update", operatingModeDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(OperatingModeEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var operatingMode = _mapper.Map<OperatingMode>(model);
            await _operatingModeService.Update(operatingMode);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var operatingMode = await _operatingModeService.GetById(id);
            if (operatingMode == null)
                return Json(new { success = false, ErrorMessage = "Operating Mode not found" });

            await _operatingModeService.Remove(operatingMode);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentOperatingMode = await _operatingModeService.GetById(request.Id);

            if (currentOperatingMode == null)
                return Json(new { success = false, ErrorMessage = "OperatingMode not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the OperatingMode to swap with (higher for move down, lower for move up)
            var swapOperatingMode = (await _operatingModeService.GetAll())
                .Where(om => isMoveUp ? om.SortOrder < currentOperatingMode.SortOrder : om.SortOrder > currentOperatingMode.SortOrder)
                .OrderBy(om => isMoveUp ? om.SortOrder * -1 : om.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapOperatingMode == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No OperatingMode to move up." : "No OperatingMode to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentOperatingMode.SortOrder;

            currentOperatingMode.SortOrder = swapOperatingMode.SortOrder;

            swapOperatingMode.SortOrder = tempSortOrder;

            // Update both records
            await _operatingModeService.Update(currentOperatingMode);

            await _operatingModeService.Update(swapOperatingMode);

            return Json(new { success = true });
        }
    }
}