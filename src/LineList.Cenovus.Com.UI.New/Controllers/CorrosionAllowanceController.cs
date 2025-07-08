using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.CorrosionAllowance;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class CorrosionAllowanceController : Controller
    {
        private readonly ICorrosionAllowanceService _corrosionAllowanceService;
        private readonly IMapper _mapper;

        private readonly CurrentUser _currentUser;

        public CorrosionAllowanceController(ICorrosionAllowanceService corrosionAllowanceService, IMapper mapper, CurrentUser currentUser)
        {
            _corrosionAllowanceService = corrosionAllowanceService ?? throw new ArgumentNullException(nameof(corrosionAllowanceService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        // Index action to list all corrosion allowances
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var corrosionAllowances = await _corrosionAllowanceService.GetAll();
            var corrosionAllowanceDtos = _mapper.Map<IEnumerable<CorrosionAllowanceResultDto>>(corrosionAllowances);
            return View(corrosionAllowanceDtos);
        }

        // Action to get data for corrosion allowance feed (used for AJAX call)
        [HttpGet]
        public async Task<JsonResult> CorrosionAllowanceFeed()
        {
            var corrosionAllowances = await _corrosionAllowanceService.GetAll();
            var corrosionAllowanceDtos = _mapper.Map<IEnumerable<CorrosionAllowanceResultDto>>(corrosionAllowances);
            return Json(new { data = corrosionAllowanceDtos });
        }

        // Action to load the create form
        [HttpGet]
        public IActionResult Create()
        {
            var model = new CorrosionAllowanceAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        // Action to handle creating a new corrosion allowance
        [HttpPost]
        public async Task<JsonResult> Create(CorrosionAllowanceAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var corrosionAllowance = _mapper.Map<CorrosionAllowance>(model);
            var newCorrosionAllowance = await _corrosionAllowanceService.Add(corrosionAllowance);

            if (newCorrosionAllowance == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        // Action to load the update form for a specific corrosion allowance
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var corrosionAllowance = await _corrosionAllowanceService.GetById(id);
            if (corrosionAllowance == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_corrosionAllowanceService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line revision, Pipe Specification", "Corrosion Allowance", corrosionAllowance.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var corrosionAllowanceDto = _mapper.Map<CorrosionAllowanceEditDto>(corrosionAllowance);
            return PartialView("_Update", corrosionAllowanceDto);
        }

        // Action to handle updating a corrosion allowance
        [HttpPost]
        public async Task<JsonResult> Update(CorrosionAllowanceEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var corrosionAllowance = _mapper.Map<CorrosionAllowance>(model);
            await _corrosionAllowanceService.Update(corrosionAllowance);

            return Json(new { success = true });
        }


        // Action to delete a corrosion allowance
        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var corrosionAllowance = await _corrosionAllowanceService.GetById(id);
            if (corrosionAllowance == null)
                return Json(new { success = false, ErrorMessage = "Corrosion Allowance not found" });

            await _corrosionAllowanceService.Remove(corrosionAllowance);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentCorrosionAllowance = await _corrosionAllowanceService.GetById(request.Id);
            if (currentCorrosionAllowance == null)
                return Json(new { success = false, ErrorMessage = "CorrosionAllowance not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the CorrosionAllowance to swap with (higher for move down, lower for move up)
            var swapCorrosionAllowance = (await _corrosionAllowanceService.GetAll())
                .Where(ca => isMoveUp ? ca.SortOrder < currentCorrosionAllowance.SortOrder : ca.SortOrder > currentCorrosionAllowance.SortOrder)
                .OrderBy(ca => isMoveUp ? ca.SortOrder * -1 : ca.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapCorrosionAllowance == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No CorrosionAllowance to move up." : "No CorrosionAllowance to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentCorrosionAllowance.SortOrder;
            currentCorrosionAllowance.SortOrder = swapCorrosionAllowance.SortOrder;
            swapCorrosionAllowance.SortOrder = tempSortOrder;

            // Update both records
            await _corrosionAllowanceService.Update(currentCorrosionAllowance);
            await _corrosionAllowanceService.Update(swapCorrosionAllowance);

            return Json(new { success = true });
        }
    }
}