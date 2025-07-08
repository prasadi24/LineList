using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Fluid;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class FluidController : Controller
    {
        private readonly IFluidService _fluidService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public FluidController(IFluidService fluidService, IMapper mapper, CurrentUser currentUser)
        {
            _fluidService = fluidService ?? throw new ArgumentNullException(nameof(fluidService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var fluids = await _fluidService.GetAll();
            var fluidDtos = _mapper.Map<IEnumerable<FluidResultDto>>(fluids);
            return View(fluidDtos);
        }

        [HttpGet]
        public async Task<JsonResult> FluidFeed()
        {
            var fluids = await _fluidService.GetAll();
            var fluidDtos = _mapper.Map<IEnumerable<FluidResultDto>>(fluids);
            return Json(new { data = fluidDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new FluidAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(FluidAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var fluid = _mapper.Map<Fluid>(model);
            var newFluid = await _fluidService.Add(fluid);

            if (newFluid == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var fluid = await _fluidService.GetById(id);
            if (fluid == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_fluidService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Operating Mode", "Fluid", fluid.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var fluidDto = _mapper.Map<FluidEditDto>(fluid);
            return PartialView("_Update", fluidDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(FluidEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var fluid = _mapper.Map<Fluid>(model);
            await _fluidService.Update(fluid);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var fluid = await _fluidService.GetById(id);
            if (fluid == null)
                return Json(new { success = false, ErrorMessage = "Fluid not found" });

            await _fluidService.Remove(fluid);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentFluid = await _fluidService.GetById(request.Id);

            if (currentFluid == null)
                return Json(new { success = false, ErrorMessage = "Fluid not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the Fluid to swap with (higher for move down, lower for move up)
            var swapFluid = (await _fluidService.GetAll())
                .Where(f => isMoveUp ? f.SortOrder < currentFluid.SortOrder : f.SortOrder > currentFluid.SortOrder)
                .OrderBy(f => isMoveUp ? f.SortOrder * -1 : f.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapFluid == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No Fluid to move up." : "No Fluid to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentFluid.SortOrder;

            currentFluid.SortOrder = swapFluid.SortOrder;

            swapFluid.SortOrder = tempSortOrder;

            // Update both records
            await _fluidService.Update(currentFluid);

            await _fluidService.Update(swapFluid);

            return Json(new { success = true });
        }
    }
}