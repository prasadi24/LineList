using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.NdeCategory;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class NDECategoryController : Controller
    {
        private readonly INdeCategoryService _ndeCategoryService;
        private readonly IMapper _mapper;

        private readonly IXrayService _xrayService;
        private readonly CurrentUser _currentUser;

        public NDECategoryController(INdeCategoryService ndeCategoryService, IMapper mapper, IXrayService xrayService, CurrentUser currentUser)
        {
            _ndeCategoryService = ndeCategoryService ?? throw new ArgumentNullException(nameof(ndeCategoryService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _xrayService = xrayService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var ndeCategories = await _ndeCategoryService.GetAll();
            var ndeCategoryDtos = _mapper.Map<IEnumerable<NdeCategoryResultDto>>(ndeCategories);
            return View(ndeCategoryDtos);
        }

        [HttpGet]
        public async Task<JsonResult> NDECategoryFeed()
        {
            var ndeCategories = await _ndeCategoryService.GetAll();
            var ndeCategoryDtos = _mapper.Map<IEnumerable<NdeCategoryResultDto>>(ndeCategories);
            return Json(new { data = ndeCategoryDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new NdeCategoryAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };

            ViewBag.Xrays = _xrayService.GetAll().Result.Where(x => x.IsActive).OrderBy(x => x.SortOrder).ToList();

            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(NdeCategoryAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var ndeCategory = _mapper.Map<NdeCategory>(model);
            var newNdeCategory = await _ndeCategoryService.Add(ndeCategory);

            if (newNdeCategory == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var ndeCategory = await _ndeCategoryService.GetById(id);
            if (ndeCategory == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_ndeCategoryService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Pipe Specification, Line Revision", "NDE Category", ndeCategory.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var ndeCategoryDto = _mapper.Map<NdeCategoryEditDto>(ndeCategory);

            ViewBag.Xrays = _xrayService.GetAll().Result.Where(x => x.IsActive).OrderBy(x => x.SortOrder).ToList();
            return PartialView("_Update", ndeCategoryDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(NdeCategoryEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var ndeCategory = _mapper.Map<NdeCategory>(model);
            await _ndeCategoryService.Update(ndeCategory);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var ndeCategory = await _ndeCategoryService.GetById(id);
            if (ndeCategory == null)
                return Json(new { success = false, ErrorMessage = "NDE Category not found" });

            await _ndeCategoryService.Remove(ndeCategory);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentNDECategory = await _ndeCategoryService.GetById(request.Id);
            if (currentNDECategory == null)
                return Json(new { success = false, ErrorMessage = "NDECategory not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the NDECategory to swap with (higher for move down, lower for move up)
            var swapNDECategory = (await _ndeCategoryService.GetAll())
                .Where(ndec => isMoveUp ? ndec.SortOrder < currentNDECategory.SortOrder : ndec.SortOrder > currentNDECategory.SortOrder)
                .OrderBy(ndec => isMoveUp ? ndec.SortOrder * -1 : ndec.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapNDECategory == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No NDECategory to move up." : "No NDECategory to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentNDECategory.SortOrder;
            currentNDECategory.SortOrder = swapNDECategory.SortOrder;
            swapNDECategory.SortOrder = tempSortOrder;

            // Update both records
            await _ndeCategoryService.Update(currentNDECategory);
            await _ndeCategoryService.Update(swapNDECategory);

            return Json(new { success = true });
        }
    }
}