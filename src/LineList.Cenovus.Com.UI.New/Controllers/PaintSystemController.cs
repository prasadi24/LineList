using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.PaintSystem;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class PaintSystemController : Controller
    {
        private readonly IPaintSystemService _paintSystemService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public PaintSystemController(IPaintSystemService paintSystemService, IMapper mapper, CurrentUser currentUser)
        {
            _paintSystemService = paintSystemService ?? throw new ArgumentNullException(nameof(paintSystemService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var paintSystems = await _paintSystemService.GetAll();
            var paintSystemDtos = _mapper.Map<IEnumerable<PaintSystemResultDto>>(paintSystems);
            return View(paintSystemDtos);
        }

        [HttpGet]
        public async Task<JsonResult> PaintSystemFeed()
        {
            var paintSystems = await _paintSystemService.GetAll();
            var paintSystemDtos = _mapper.Map<IEnumerable<PaintSystemResultDto>>(paintSystems);
            return Json(new { data = paintSystemDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new PaintSystemAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(PaintSystemAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var paintSystem = _mapper.Map<PaintSystem>(model);
            var newPaintSystem = await _paintSystemService.Add(paintSystem);

            if (newPaintSystem == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var paintSystem = await _paintSystemService.GetById(id);
            if (paintSystem == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_paintSystemService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Segment", "Paint System", paintSystem.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var paintSystemDto = _mapper.Map<PaintSystemEditDto>(paintSystem);
            return PartialView("_Update", paintSystemDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(PaintSystemEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var paintSystem = _mapper.Map<PaintSystem>(model);
            await _paintSystemService.Update(paintSystem);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var paintSystem = await _paintSystemService.GetById(id);
            if (paintSystem == null)
                return Json(new { success = false, ErrorMessage = "Paint System not found" });

            await _paintSystemService.Remove(paintSystem);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentPaintSystem = await _paintSystemService.GetById(request.Id);

            if (currentPaintSystem == null)
                return Json(new { success = false, ErrorMessage = "PaintSystem not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the PaintSystem to swap with (higher for move down, lower for move up)
            var swapPaintSystem = (await _paintSystemService.GetAll())
                .Where(ps => isMoveUp ? ps.SortOrder < currentPaintSystem.SortOrder : ps.SortOrder > currentPaintSystem.SortOrder)
                .OrderBy(ps => isMoveUp ? ps.SortOrder * -1 : ps.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapPaintSystem == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No PaintSystem to move up." : "No PaintSystem to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentPaintSystem.SortOrder;

            currentPaintSystem.SortOrder = swapPaintSystem.SortOrder;

            swapPaintSystem.SortOrder = tempSortOrder;

            // Update both records
            await _paintSystemService.Update(currentPaintSystem);

            await _paintSystemService.Update(swapPaintSystem);

            return Json(new { success = true });
        }
    }
}