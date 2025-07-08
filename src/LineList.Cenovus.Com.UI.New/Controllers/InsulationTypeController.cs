using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationType;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InsulationTypeController : Controller
    {
        private readonly IInsulationTypeService _insulationTypeService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public InsulationTypeController(IInsulationTypeService insulationTypeService, IMapper mapper, CurrentUser currentUser)
        {
            _insulationTypeService = insulationTypeService ?? throw new ArgumentNullException(nameof(insulationTypeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var insulationTypes = await _insulationTypeService.GetAll();
            var insulationTypeDtos = _mapper.Map<IEnumerable<InsulationTypeResultDto>>(insulationTypes);
            return View(insulationTypeDtos);
        }

        [HttpGet]
        public async Task<JsonResult> InsulationTypeFeed()
        {
            var insulationTypes = await _insulationTypeService.GetAll();
            var insulationTypeDtos = _mapper.Map<IEnumerable<InsulationTypeResultDto>>(insulationTypes);
            return Json(new { data = insulationTypeDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new InsulationTypeAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(InsulationTypeAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var insulationType = _mapper.Map<InsulationType>(model);
            var newInsulationType = await _insulationTypeService.Add(insulationType);
            if (newInsulationType == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var insulationType = await _insulationTypeService.GetById(id);
            if (insulationType == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_insulationTypeService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line revision Segment, Insulation Default Detail", "Insulation Type", insulationType.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var insulationTypeDto = _mapper.Map<InsulationTypeEditDto>(insulationType);
            return PartialView("_Update", insulationTypeDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InsulationTypeEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var insulationType = _mapper.Map<InsulationType>(model);
            await _insulationTypeService.Update(insulationType);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var insulationType = await _insulationTypeService.GetById(id);
            if (insulationType == null)
                return Json(new { success = false, ErrorMessage = "Insulation Type not found" });

            await _insulationTypeService.Remove(insulationType);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentInsulationType = await _insulationTypeService.GetById(request.Id);
            if (currentInsulationType == null)
                return Json(new { success = false, ErrorMessage = "InsulationType not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the InsulationType to swap with (higher for move down, lower for move up)
            var swapInsulationType = (await _insulationTypeService.GetAll())
                .Where(it => isMoveUp ? it.SortOrder < currentInsulationType.SortOrder : it.SortOrder > currentInsulationType.SortOrder)
                .OrderBy(it => isMoveUp ? it.SortOrder * -1 : it.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapInsulationType == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No InsulationType to move up." : "No InsulationType to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentInsulationType.SortOrder;
            currentInsulationType.SortOrder = swapInsulationType.SortOrder;
            swapInsulationType.SortOrder = tempSortOrder;

            // Update both records
            await _insulationTypeService.Update(currentInsulationType);
            await _insulationTypeService.Update(swapInsulationType);

            return Json(new { success = true });
        }
    }
}