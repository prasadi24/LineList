using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.Xray;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class XrayController : Controller
    {
        private readonly IXrayService _xrayService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public XrayController(IXrayService xrayService, IMapper mapper, CurrentUser currentUser)
        {
            _xrayService = xrayService ?? throw new ArgumentNullException(nameof(xrayService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var xrays = await _xrayService.GetAll();
            var xrayDtos = _mapper.Map<IEnumerable<XrayResultDto>>(xrays);
            return View(xrayDtos);
        }

        [HttpGet]
        public async Task<JsonResult> XrayFeed()
        {
            var xrays = await _xrayService.GetAll();
            var xrayDtos = _mapper.Map<IEnumerable<XrayResultDto>>(xrays);
            return Json(new { data = xrayDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new XrayAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(XrayAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var xray = _mapper.Map<Xray>(model);
            var newXray = await _xrayService.Add(xray);
            if (newXray == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var xray = await _xrayService.GetById(id);
            if (xray == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_xrayService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing NDE Category, Pipe Specification, Line Revision", "X-Ray", xray.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var xrayDto = _mapper.Map<XrayEditDto>(xray);
            return PartialView("_Update", xrayDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(XrayEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var xray = _mapper.Map<Xray>(model);
            await _xrayService.Update(xray);

            return Json(new { success = true });
        }


        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var xray = await _xrayService.GetById(id);
            if (xray == null)
                return Json(new { success = false, ErrorMessage = "Xray not found" });

            await _xrayService.Remove(xray);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentXray = await _xrayService.GetById(request.Id);

            if (currentXray == null)
                return Json(new { success = false, ErrorMessage = "Xray not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the Xray to swap with (higher for move down, lower for move up)
            var swapXray = (await _xrayService.GetAll())
                .Where(x => isMoveUp ? x.SortOrder < currentXray.SortOrder : x.SortOrder > currentXray.SortOrder)
                .OrderBy(x => isMoveUp ? x.SortOrder * -1 : x.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapXray == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No Xray to move up." : "No Xray to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentXray.SortOrder;

            currentXray.SortOrder = swapXray.SortOrder;

            swapXray.SortOrder = tempSortOrder;

            // Update both records
            await _xrayService.Update(currentXray);

            await _xrayService.Update(swapXray);

            return Json(new { success = true });
        }
    }
}