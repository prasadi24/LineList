using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.FluidPhase;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class FluidPhaseController : Controller
    {
        private readonly IFluidPhaseService _fluidPhaseService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public FluidPhaseController(IFluidPhaseService fluidPhaseService, IMapper mapper, CurrentUser currentUser)
        {
            _fluidPhaseService = fluidPhaseService ?? throw new ArgumentNullException(nameof(fluidPhaseService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fluidPhases = await _fluidPhaseService.GetAll();
            var fluidPhaseDtos = _mapper.Map<IEnumerable<FluidPhaseResultDto>>(fluidPhases);
            return View(fluidPhaseDtos);
        }

        [HttpGet]
        public async Task<JsonResult> FluidPhaseFeed()
        {
            var fluidPhases = await _fluidPhaseService.GetAll();
            var fluidPhaseDtos = _mapper.Map<IEnumerable<FluidPhaseResultDto>>(fluidPhases);
            return Json(new { data = fluidPhaseDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new FluidPhaseAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(FluidPhaseAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var fluidPhase = _mapper.Map<FluidPhase>(model);
            var newFluidPhase = await _fluidPhaseService.Add(fluidPhase);

            if (newFluidPhase == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var fluidPhase = await _fluidPhaseService.GetById(id);
            if (fluidPhase == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_fluidPhaseService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Operating Mode", "Fluid Phase", fluidPhase.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var fluidPhaseDto = _mapper.Map<FluidPhaseEditDto>(fluidPhase);
            return PartialView("_Update", fluidPhaseDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(FluidPhaseEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var fluidPhase = _mapper.Map<FluidPhase>(model);
            await _fluidPhaseService.Update(fluidPhase);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var fluidPhase = await _fluidPhaseService.GetById(id);
            if (fluidPhase == null)
                return Json(new { success = false, ErrorMessage = "Fluid Phase not found" });

            await _fluidPhaseService.Remove(fluidPhase);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentFluidPhase = await _fluidPhaseService.GetById(request.Id);
            if (currentFluidPhase == null)
                return Json(new { success = false, ErrorMessage = "FluidPhase not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the FluidPhase to swap with (higher for move down, lower for move up)
            var swapFluidPhase = (await _fluidPhaseService.GetAll())
                .Where(fp => isMoveUp ? fp.SortOrder < currentFluidPhase.SortOrder : fp.SortOrder > currentFluidPhase.SortOrder)
                .OrderBy(fp => isMoveUp ? fp.SortOrder * -1 : fp.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapFluidPhase == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No FluidPhase to move up." : "No FluidPhase to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentFluidPhase.SortOrder;
            currentFluidPhase.SortOrder = swapFluidPhase.SortOrder;
            swapFluidPhase.SortOrder = tempSortOrder;

            // Update both records
            await _fluidPhaseService.Update(currentFluidPhase);
            await _fluidPhaseService.Update(swapFluidPhase);

            return Json(new { success = true });
        }
    }
}