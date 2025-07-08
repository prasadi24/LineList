using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Commodity;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class CommodityController : Controller
    {
        private readonly ICommodityService _commodityService;
        private readonly IMapper _mapper;

        private readonly ISpecificationService _specificationService;
        private readonly CurrentUser _currentUser;

        public CommodityController(ICommodityService commodityService, IMapper mapper, ISpecificationService specificationService, CurrentUser currentUser)
        {
            _commodityService = commodityService ?? throw new ArgumentNullException(nameof(commodityService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _specificationService = specificationService;
            _currentUser = currentUser;
        }

        // Index action to list all commodities
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var commodities = await _commodityService.GetAll();
            var commodityDtos = _mapper.Map<IEnumerable<CommodityResultDto>>(commodities);

            ViewBag.Specifications = await _specificationService.GetAll();
            return View(commodityDtos);
        }

        // Action to get data for commodity feed (used for AJAX call)
        [HttpGet]
        public async Task<JsonResult> CommodityFeed()
        {
            var commodities = await _commodityService.GetAll();
            var commodityDtos = _mapper.Map<IEnumerable<CommodityResultDto>>(commodities);
            return Json(new { data = commodityDtos });
        }

        // Action to load the create form
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CommodityAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            ViewBag.Specifications = await _specificationService.GetAll();
            return PartialView("_Create", model);
        }

        // Action to handle creating a new commodity
        [HttpPost]
        public async Task<JsonResult> Create(CommodityAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var commodity = _mapper.Map<Commodity>(model);
            var newCommodity = await _commodityService.Add(commodity);

            if (newCommodity == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        // Action to load the update form for a specific commodity
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var commodity = await _commodityService.GetById(id);
            if (commodity == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_commodityService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Line, Line Revision", "Commodity", commodity.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var commodityDto = _mapper.Map<CommodityEditDto>(commodity);

            ViewBag.Specifications = await _specificationService.GetAll();
            return PartialView("_Update", commodityDto);
        }

        // Action to handle updating a commodity
        [HttpPost]
        public async Task<JsonResult> Update(CommodityEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var commodity = _mapper.Map<Commodity>(model);
            var updatedCommodity = await _commodityService.Update(commodity);

            if (updatedCommodity == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        // Action to delete a commodity
        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var commodity = await _commodityService.GetById(id);
            if (commodity == null)
                return Json(new { success = false, ErrorMessage = "Commodity not found" });

            await _commodityService.Remove(commodity);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentCommodity = await _commodityService.GetById(request.Id);
            if (currentCommodity == null)
                return Json(new { success = false, ErrorMessage = "Commodity not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the commodity to swap with (higher for move down, lower for move up)
            var swapCommodity = (await _commodityService.GetAll())
                .Where(c => isMoveUp ? c.SortOrder < currentCommodity.SortOrder : c.SortOrder > currentCommodity.SortOrder)
                .OrderBy(c => isMoveUp ? c.SortOrder * -1 : c.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapCommodity == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No commodity to move up." : "No commodity to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentCommodity.SortOrder;
            currentCommodity.SortOrder = swapCommodity.SortOrder;
            swapCommodity.SortOrder = tempSortOrder;

            // Update both records
            await _commodityService.Update(currentCommodity);
            await _commodityService.Update(swapCommodity);

            return Json(new { success = true });
        }
    }
}