using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.InsulationMaterial;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class InsulationMaterialController : Controller
    {
        private readonly IInsulationMaterialService _insulationMaterialService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public InsulationMaterialController(IInsulationMaterialService insulationMaterialService, IMapper mapper, CurrentUser currentUser)
        {
            _insulationMaterialService = insulationMaterialService ?? throw new ArgumentNullException(nameof(insulationMaterialService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var insulationMaterials = await _insulationMaterialService.GetAll();
            var insulationMaterialDtos = _mapper.Map<IEnumerable<InsulationMaterialResultDto>>(insulationMaterials);
            return View(insulationMaterialDtos);
        }

        [HttpGet]
        public async Task<JsonResult> InsulationMaterialFeed()
        {
            var insulationMaterials = await _insulationMaterialService.GetAll();
            var insulationMaterialDtos = _mapper.Map<IEnumerable<InsulationMaterialResultDto>>(insulationMaterials);
            return Json(new { data = insulationMaterialDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new InsulationMaterialAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(InsulationMaterialAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var insulationMaterial = _mapper.Map<InsulationMaterial>(model);
            var newInsulationMaterial = await _insulationMaterialService.Add(insulationMaterial);

            if (newInsulationMaterial == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var insulationMaterial = await _insulationMaterialService.GetById(id);
            if (insulationMaterial == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_insulationMaterialService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line Revision Segment, Insulation Default", "Insulation Material", insulationMaterial.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var insulationMaterialDto = _mapper.Map<InsulationMaterialEditDto>(insulationMaterial);
            return PartialView("_Update", insulationMaterialDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(InsulationMaterialEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var insulationMaterial = _mapper.Map<InsulationMaterial>(model);
            await _insulationMaterialService.Update(insulationMaterial);

            return Json(new { success = true });
        }               

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var insulationMaterial = await _insulationMaterialService.GetById(id);
            if (insulationMaterial == null)
                return Json(new { success = false, ErrorMessage = "Insulation Material not found" });

            await _insulationMaterialService.Remove(insulationMaterial);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentInsulationMaterial = await _insulationMaterialService.GetById(request.Id);
            if (currentInsulationMaterial == null)
                return Json(new { success = false, ErrorMessage = "InsulationMaterial not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the InsulationMaterial to swap with (higher for move down, lower for move up)
            var swapInsulationMaterial = (await _insulationMaterialService.GetAll())
                .Where(im => isMoveUp ? im.SortOrder < currentInsulationMaterial.SortOrder : im.SortOrder > currentInsulationMaterial.SortOrder)
                .OrderBy(im => isMoveUp ? im.SortOrder * -1 : im.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapInsulationMaterial == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No InsulationMaterial to move up." : "No InsulationMaterial to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentInsulationMaterial.SortOrder;
            currentInsulationMaterial.SortOrder = swapInsulationMaterial.SortOrder;
            swapInsulationMaterial.SortOrder = tempSortOrder;

            // Update both records
            await _insulationMaterialService.Update(currentInsulationMaterial);
            await _insulationMaterialService.Update(swapInsulationMaterial);

            return Json(new { success = true });
        }
    }
}