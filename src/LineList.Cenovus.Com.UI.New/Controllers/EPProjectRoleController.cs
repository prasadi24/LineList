using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectRole;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class EpProjectRoleController : Controller
    {
        private readonly IEpProjectRoleService _epProjectRoleService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public EpProjectRoleController(IEpProjectRoleService epProjectRoleService, IMapper mapper, CurrentUser currentUser)
        {
            _epProjectRoleService = epProjectRoleService ?? throw new ArgumentNullException(nameof(epProjectRoleService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewData["IsCenovusAdmin"] = _currentUser.IsCenovusAdmin;
            ViewData["IsEpAdmin"] = _currentUser.IsEpAdmin;
            var epProjectRoles = await _epProjectRoleService.GetAll();
            var epProjectRoleDtos = _mapper.Map<IEnumerable<EpProjectRoleResultDto>>(epProjectRoles);
            return View(epProjectRoleDtos);
        }

        [HttpGet]
        public async Task<JsonResult> EpProjectRoleFeed()
        {
            var epProjectRoles = await _epProjectRoleService.GetAll();
            var epProjectRoleDtos = _mapper.Map<IEnumerable<EpProjectRoleResultDto>>(epProjectRoles);
            return Json(new { data = epProjectRoleDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new EpProjectRoleAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(EpProjectRoleAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var epProjectRole = _mapper.Map<EpProjectRole>(model);
            var newEpProjectRole = await _epProjectRoleService.Add(epProjectRole);

            if (newEpProjectRole == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var epProjectRole = await _epProjectRoleService.GetById(id);
            if (epProjectRole == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_epProjectRoleService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing EP Project User Role", "EP Project Role", epProjectRole.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var epProjectRoleDto = _mapper.Map<EpProjectRoleEditDto>(epProjectRole);
            return PartialView("_Update", epProjectRoleDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(EpProjectRoleEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var epProjectRole = _mapper.Map<EpProjectRole>(model);
            await _epProjectRoleService.Update(epProjectRole);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var epProjectRole = await _epProjectRoleService.GetById(id);
            if (epProjectRole == null)
                return Json(new { success = false, ErrorMessage = "EpProject Role not found" });           

            await _epProjectRoleService.Remove(epProjectRole);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var epProjectRole = await _epProjectRoleService.GetById(request.Id);
            if (epProjectRole == null)
                return Json(new { success = false, ErrorMessage = "EP Project Role not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the EP Project  to swap with (higher for move down, lower for move up)
            var swapProjectRole = (await _epProjectRoleService.GetAll())
                .Where(im => isMoveUp ? im.SortOrder < epProjectRole.SortOrder : im.SortOrder > epProjectRole.SortOrder)
                .OrderBy(im => isMoveUp ? im.SortOrder * -1 : im.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapProjectRole == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No InsulationMaterial to move up." : "No InsulationMaterial to move down." });

            // Swap SortOrder values
            int tempSortOrder = epProjectRole.SortOrder;
            epProjectRole.SortOrder = swapProjectRole.SortOrder;
            swapProjectRole.SortOrder = tempSortOrder;

            // Update both records
            await _epProjectRoleService.Update(epProjectRole);
            await _epProjectRoleService.Update(swapProjectRole);

            return Json(new { success = true });
        }
    }
}