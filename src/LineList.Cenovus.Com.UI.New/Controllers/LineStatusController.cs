using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.LineStatus;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourNamespace.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class LineStatusController : Controller
    {
        private readonly ILineStatusService _lineStatusService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public LineStatusController(ILineStatusService lineStatusService, IMapper mapper, CurrentUser currentUser)
        {
            _lineStatusService = lineStatusService ?? throw new ArgumentNullException(nameof(lineStatusService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        // GET: LineStatus
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var lineStatuses = await _lineStatusService.GetAll();
            var lineStatusDtos = _mapper.Map<IEnumerable<LineStatusResultDto>>(lineStatuses);
            return View(lineStatusDtos);
        }

        // GET: LineStatus Feed for DataTables (optional)
        [HttpGet]
        public async Task<JsonResult> LineStatusFeed()
        {
            var lineStatuses = await _lineStatusService.GetAll();
            var lineStatusDtos = _mapper.Map<IEnumerable<LineStatusResultDto>>(lineStatuses);
            return Json(new { data = lineStatusDtos });
        }

        // GET: LineStatus/Create
        [HttpGet]
        public IActionResult Create()
        {
            var model = new LineStatusAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        // POST: LineStatus/Create
        [HttpPost]
        public async Task<JsonResult> Create(LineStatusAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var lineStatus = _mapper.Map<LineStatus>(model);
            var newLineStatus = await _lineStatusService.Add(lineStatus);

            if (newLineStatus == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        // GET: LineStatus/Update/{id}
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var lineStatus = await _lineStatusService.GetById(id);
            if (lineStatus == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_lineStatusService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision", "Line Status", lineStatus.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var lineStatusDto = _mapper.Map<LineStatusEditDto>(lineStatus);
            return PartialView("_Update", lineStatusDto);
        }

        // POST: LineStatus/Update
        [HttpPost]
        public async Task<JsonResult> Update(LineStatusEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var lineStatus = _mapper.Map<LineStatus>(model);
            await _lineStatusService.Update(lineStatus);

            return Json(new { success = true });
        }

        // DELETE: LineStatus/Delete/{id}
        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var lineStatus = await _lineStatusService.GetById(id);
            if (lineStatus == null)
                return Json(new { success = false, ErrorMessage = "Line Status not found" });

            await _lineStatusService.Remove(lineStatus);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentLineStatus = await _lineStatusService.GetById(request.Id);

            if (currentLineStatus == null)
                return Json(new { success = false, ErrorMessage = "LineStatus not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the LineStatus to swap with (higher for move down, lower for move up)
            var swapLineStatus = (await _lineStatusService.GetAll())
                .Where(ls => isMoveUp ? ls.SortOrder < currentLineStatus.SortOrder : ls.SortOrder > currentLineStatus.SortOrder)
                .OrderBy(ls => isMoveUp ? ls.SortOrder * -1 : ls.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapLineStatus == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No LineStatus to move up." : "No LineStatus to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentLineStatus.SortOrder;

            currentLineStatus.SortOrder = swapLineStatus.SortOrder;

            swapLineStatus.SortOrder = tempSortOrder;

            // Update both records
            await _lineStatusService.Update(currentLineStatus);

            await _lineStatusService.Update(swapLineStatus);

            return Json(new { success = true });
        }
    }
}