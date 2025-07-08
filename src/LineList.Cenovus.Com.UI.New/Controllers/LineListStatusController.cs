using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.LineListStatus;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class LineListStatusController : Controller
    {
        private readonly ILineListStatusService _lineListStatusService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public LineListStatusController(ILineListStatusService lineListStatusService, IMapper mapper, CurrentUser currentUser)
        {
            _lineListStatusService = lineListStatusService ?? throw new ArgumentNullException(nameof(lineListStatusService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lineListStatuses = await _lineListStatusService.GetAll();
            var lineListStatusDtos = _mapper.Map<IEnumerable<LineListStatusResultDto>>(lineListStatuses);
            return View(lineListStatusDtos);
        }

        [HttpGet]
        public async Task<JsonResult> LineListStatusFeed()
        {
            var lineListStatuses = await _lineListStatusService.GetAll();
            var lineListStatusDtos = _mapper.Map<IEnumerable<LineListStatusResultDto>>(lineListStatuses);
            return Json(new { data = lineListStatusDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new LineListStatusAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };

            ViewBag.lineListStatuses = await _lineListStatusService.GetAll();

            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(LineListStatusAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var lineListStatus = _mapper.Map<LineListStatus>(model);
            var newLineListStatus = await _lineListStatusService.Add(lineListStatus);

            if (newLineListStatus == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }


            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var lineListStatus = await _lineListStatusService.GetById(id);
            if (lineListStatus == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_lineListStatusService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision, Line List Status, Line List Status State", "Line List Status", lineListStatus.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var lineListStatusDto = _mapper.Map<LineListStatusEditDto>(lineListStatus);

            ViewBag.lineListStatuses = await _lineListStatusService.GetAll();
            return PartialView("_Update", lineListStatusDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(LineListStatusEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var lineListStatus = _mapper.Map<LineListStatus>(model);
            await _lineListStatusService.Update(lineListStatus);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var lineListStatus = await _lineListStatusService.GetById(id);
            if (lineListStatus == null)
                return Json(new { success = false, ErrorMessage = "Line List Status not found" });           

            await _lineListStatusService.Remove(lineListStatus);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentLineListStatus = await _lineListStatusService.GetById(request.Id);

            if (currentLineListStatus == null)
                return Json(new { success = false, ErrorMessage = "LineListStatus not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the LineListStatus to swap with (higher for move down, lower for move up)
            var swapLineListStatus = (await _lineListStatusService.GetAll())
                .Where(lls => isMoveUp ? lls.SortOrder < currentLineListStatus.SortOrder : lls.SortOrder > currentLineListStatus.SortOrder)
                .OrderBy(lls => isMoveUp ? lls.SortOrder * -1 : lls.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapLineListStatus == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No LineListStatus to move up." : "No LineListStatus to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentLineListStatus.SortOrder;

            currentLineListStatus.SortOrder = swapLineListStatus.SortOrder;

            swapLineListStatus.SortOrder = tempSortOrder;

            // Update both records
            await _lineListStatusService.Update(currentLineListStatus);

            await _lineListStatusService.Update(swapLineListStatus);

            return Json(new { success = true });
        }
    }
}