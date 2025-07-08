using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationThickness;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InsulationThicknessController : Controller
    {
        private readonly IInsulationThicknessService _insulationThicknessService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public InsulationThicknessController(IInsulationThicknessService insulationThicknessService, IMapper mapper, CurrentUser currentUser)
        {
            _insulationThicknessService = insulationThicknessService ?? throw new ArgumentNullException(nameof(insulationThicknessService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var insulationThicknesses = await _insulationThicknessService.GetAll();
            var insulationThicknessDtos = _mapper.Map<IEnumerable<InsulationThicknessResultDto>>(insulationThicknesses);
            return View(insulationThicknessDtos);
        }

        [HttpGet]
        public async Task<JsonResult> InsulationThicknessFeed()
        {
            var insulationThicknesses = await _insulationThicknessService.GetAll();
            var insulationThicknessDtos = _mapper.Map<IEnumerable<InsulationThicknessResultDto>>(insulationThicknesses);
            return Json(new { data = insulationThicknessDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new InsulationThicknessAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(InsulationThicknessAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var insulationThickness = _mapper.Map<InsulationThickness>(model);
            var newInsulationThickness = await _insulationThicknessService.Add(insulationThickness);

            if (newInsulationThickness == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }


            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var insulationThickness = await _insulationThicknessService.GetById(id);
            if (insulationThickness == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_insulationThicknessService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Insulation Default Detail", "Insulation", insulationThickness.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var insulationThicknessDto = _mapper.Map<InsulationThicknessEditDto>(insulationThickness);
            return PartialView("_Update", insulationThicknessDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InsulationThicknessEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var insulationThickness = _mapper.Map<InsulationThickness>(model);
            var updateInsulationThickness = await _insulationThicknessService.Update(insulationThickness);

            if (updateInsulationThickness == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var insulationThickness = await _insulationThicknessService.GetById(id);
            if (insulationThickness == null)
                return Json(new { success = false, ErrorMessage = "Insulation Thickness not found" });

            await _insulationThicknessService.Remove(insulationThickness);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentInsulationThickness = await _insulationThicknessService.GetById(request.Id);
            if (currentInsulationThickness == null)
                return Json(new { success = false, ErrorMessage = "InsulationThickness not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the InsulationThickness to swap with (higher for move down, lower for move up)
            var swapInsulationThickness = (await _insulationThicknessService.GetAll())
                .Where(it => isMoveUp ? it.SortOrder < currentInsulationThickness.SortOrder : it.SortOrder > currentInsulationThickness.SortOrder)
                .OrderBy(it => isMoveUp ? it.SortOrder * -1 : it.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapInsulationThickness == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No InsulationThickness to move up." : "No InsulationThickness to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentInsulationThickness.SortOrder;
            currentInsulationThickness.SortOrder = swapInsulationThickness.SortOrder;
            swapInsulationThickness.SortOrder = tempSortOrder;

            // Update both records
            await _insulationThicknessService.Update(currentInsulationThickness);
            await _insulationThicknessService.Update(swapInsulationThickness);

            return Json(new { success = true });
        }
    }
}