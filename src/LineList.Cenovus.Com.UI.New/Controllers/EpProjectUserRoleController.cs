using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProjectUserRole;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using LineList.Cenovus.Com.UI.Security;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    public class EpProjectUserRoleController : Controller
    {
        private readonly IEpProjectUserRoleService _epProjectUserRoleService;
        private readonly IEpProjectService _epProjectService;
        private readonly IEpCompanyService _epCompanyService;
        private readonly IEpProjectRoleService _epProjectRoleService;
        private readonly UserManager _userManager;
        private readonly CurrentUser _currentUser;
        private readonly IMapper _mapper;
        private readonly AzureAdService _azureAdService;
        private readonly IConfiguration _configuration;

        public EpProjectUserRoleController(IEpProjectUserRoleService epProjectUserRoleService,
            IEpProjectService epProjectService,
            IEpCompanyService epCompanyService,
            IEpProjectRoleService epProjectRoleService,
            UserManager userManager,
            CurrentUser currentUser,
            AzureAdService azureAdService,
            IConfiguration configuration,
            IMapper mapper)
        {
            _epProjectUserRoleService = epProjectUserRoleService;
            _mapper = mapper;
            _epProjectService = epProjectService;
            _epCompanyService = epCompanyService;
            _epProjectRoleService = epProjectRoleService;
            _currentUser = currentUser;
            _userManager = userManager;
            _azureAdService = azureAdService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var epProjectUserRoles = await _epProjectUserRoleService.GetAll();
            var epProjectUserRoleDtos = _mapper.Map<IEnumerable<EpProjectUserRoleResultDto>>(epProjectUserRoles);
            return View(epProjectUserRoleDtos);
        }

        [HttpGet]
        public async Task<JsonResult> EpProjectUserRoleFeed()
        {
            var epProjectUserRoles = await _epProjectUserRoleService.GetAll();
            var epProjectUserRoleDtos = _mapper.Map<IEnumerable<EpProjectUserRoleResultDto>>(epProjectUserRoles);
            return Json(new { data = epProjectUserRoleDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create(Guid id)
        {
            var epProject = await _epProjectService.GetById(id);
            if (epProject == null)
                return NotFound();

            // NEW: Fetch users for the EP
            var epCompany = await _epCompanyService.GetById(epProject.EpCompanyId);
            if (epCompany == null || string.IsNullOrEmpty(epCompany.ActiveDirectoryGroup))
                return BadRequest("EP Company or Active Directory Group not found");


            var template = _configuration.GetValue<string>("ConfigKeys:ADRoleEpUser");
            if (string.IsNullOrEmpty(template))
                     return StatusCode(500, "Configuration error: ConfigKeys:RoleEpUser is missing");
            
            //var adGroupName = string.Format(template, epCompany.ActiveDirectoryGroup);
            var adGroupName = template.Replace("{0}", epCompany.ActiveDirectoryGroup);

            //var users = await _userManager.GetUsersForEpAsync(epCompany.ActiveDirectoryGroup, false);

            var users = await _azureAdService.GetUsersFromGroup(adGroupName);
            ViewBag.Users = users; // KeyValuePair<string, string> (username, full name)

            var model = new EpProjectUserRoleAddDto
            {
                EpCompanyName = epCompany.Name,
                EpProjectName = epProject.Name,
                EpProjectId = epProject.Id,
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow
            };
            ViewBag.ProjectRole =  _epProjectRoleService.GetAll().Result.Where(r=>r.IsActive).OrderBy(r=>r.SortOrder).ToList();

            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(EpProjectUserRoleAddDto model)
        {
           
            //if (!ModelState.IsValid)
            //    return Json(new { success = false, ErrorMessage = "Model is not valid" });

            // NEW: Validate role uniqueness
            var exists = await _epProjectUserRoleService.UserInRole(model.EpProjectId, model.UserName);
            if (exists)
                return Json(new { success = false, ErrorMessage = "User already has a role for this EP Project" });

            var epProjectUserRole = _mapper.Map<EpProjectUserRole>(model);
            epProjectUserRole.CreatedBy = _currentUser.FullName; // NEW: Set audit fields
            epProjectUserRole.ModifiedBy = _currentUser.FullName;
            epProjectUserRole.CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            epProjectUserRole.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            epProjectUserRole.EpProject = null;

            await _epProjectUserRoleService.Add(epProjectUserRole);

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var epProjectUserRole = await _epProjectUserRoleService.GetById(id);
            if (epProjectUserRole == null)
                return NotFound();

            // NEW: Fetch EpProject and EpCompany
            var epProject = await _epProjectService.GetById(epProjectUserRole.EpProjectId);
            if (epProject == null)
                return NotFound();

            var epCompany = await _epCompanyService.GetById(epProject.EpCompanyId);
            if (epCompany == null || string.IsNullOrEmpty(epCompany.ActiveDirectoryGroup))
                return BadRequest("EP Company or Active Directory Group not found");

            var template = _configuration.GetValue<string>("ConfigKeys:ADRoleEpUser");
            if (string.IsNullOrEmpty(template))
                return StatusCode(500, "Configuration error: ConfigKeys:RoleEpUser is missing");

            var adGroupName = template.Replace("{0}", epCompany.ActiveDirectoryGroup);

            var users = await _azureAdService.GetUsersFromGroup(adGroupName);
            //var users = await _userManager.GetUsersForEpAsync(adGroupName, false);
            ViewBag.Users = users;

            var epProjectUserRoleDto = _mapper.Map<EpProjectUserRoleEditDto>(epProjectUserRole);
            epProjectUserRoleDto.EpCompanyName = epCompany.Name;
            epProjectUserRoleDto.EpProjectName = epProject.Name;
            epProjectUserRoleDto.EpProjectId = epProject.Id;
            ViewBag.ProjectRole = _epProjectRoleService.GetAll().Result.Where(r => r.IsActive).OrderBy(r => r.SortOrder).ToList();

            return PartialView("_Update", epProjectUserRoleDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(EpProjectUserRoleEditDto model)
        {
            if (string.IsNullOrEmpty(model.UserName) || model.EpProjectRoleId == Guid.Empty)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            // NEW: Validate role uniqueness (exclude current role)
            var exists = await _epProjectUserRoleService.UserInRole(model.EpProjectId, model.UserName, model.Id);
            if (exists)
                return Json(new { success = false, ErrorMessage = "User already has a role for this EP Project" });

            var epProjectUserRole = await _epProjectUserRoleService.GetById(model.Id);
            if (epProjectUserRole == null)
                return Json(new { success = false, ErrorMessage = "EpProjectUserRole not found" });

            _mapper.Map(model, epProjectUserRole);
            EpProjectUserRole role = new EpProjectUserRole();
            role.CreatedBy = model.CreatedBy;
            role.CreatedOn = model.CreatedOn;
            role.UserName = model.UserName.Trim();
            role.EpProjectRoleId = model.EpProjectRoleId;
            role.ModifiedBy = _currentUser.FullName;
            role.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            role.Id = model.Id;
            role.EpProjectId = model.EpProjectId;

            await _epProjectUserRoleService.Update(role);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var epProjectUserRole = await _epProjectUserRoleService.GetById(id);
            if (epProjectUserRole == null)
                return Json(new { success = false, ErrorMessage = "EpProject User Role not found" });

            await _epProjectUserRoleService.Remove(epProjectUserRole);
            return Json(new { success = true });
        }

        // NEW: Action for client-side uniqueness check
        [HttpPost]
        public async Task<JsonResult> CheckUserInRole(Guid epProjectId, string userName)
        {
            var exists = await _epProjectUserRoleService.UserInRole(epProjectId, userName);
            return Json(new { exists });
        }
    }
}