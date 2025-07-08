using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.CenovusProject;
using LineList.Cenovus.Com.Domain.DataTransferObjects;

//using LineList.Cenovus.Com.API.DataTransferObjects.ProjectGroup;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ProjectGroupController : Controller
    {
        private readonly ICenovusProjectService _projectGroupService;
        private readonly IMapper _mapper;
        private readonly IFacilityService _facilityService;  // New service for facilities
        private readonly IProjectTypeService _projectTypeService;
        private readonly CurrentUser _currentUser;

        public ProjectGroupController(ICenovusProjectService projectGroupService, IMapper mapper, IFacilityService facilityService,
        IProjectTypeService projectTypeService, CurrentUser currentUser)
        {
            _projectGroupService = projectGroupService ?? throw new ArgumentNullException(nameof(projectGroupService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _facilityService = facilityService;
            _projectTypeService = projectTypeService;
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projects = await _projectGroupService.GetAll();
            var projectDtos = _mapper.Map<IEnumerable<CenovusProjectResultDto>>(projects);
            return View(projectDtos);
        }

        [HttpGet]
        public async Task<JsonResult> ProjectGroupFeed()
        {
            var projects = await _projectGroupService.GetAll();
            var projectDtos = _mapper.Map<IEnumerable<CenovusProjectResultDto>>(projects);
            return Json(new { data = projectDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CenovusProjectAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };

            ViewBag.Facilities = await _facilityService.GetAll();
            ViewBag.ProjectTypes = await _projectTypeService.GetAll();

            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(CenovusProjectAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var project = _mapper.Map<CenovusProject>(model);
            var newProject = await _projectGroupService.Add(project);

            if (newProject == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }

            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var project = await _projectGroupService.GetById(id);
            if (project == null)
                return NotFound();

            var projectDto = _mapper.Map<CenovusProjectEditDto>(project);

            ViewBag.Facilities = await _facilityService.GetAll();
            ViewBag.ProjectTypes = await _projectTypeService.GetAll();

            return PartialView("_Update", projectDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(CenovusProjectEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var project = _mapper.Map<CenovusProject>(model);
            await _projectGroupService.Update(project);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var project = await _projectGroupService.GetById(id);
            if (project == null)
                return Json(new { success = false, ErrorMessage = "Project Group not found" });

            string message = "";
            if (_projectGroupService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing EP Project", "Project Group / Expansion Phase", project.Name_dash_Description);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                return Json(new { success = false, ErrorMessage = message });
            }

            await _projectGroupService.Remove(project);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentProjectGroup = await _projectGroupService.GetById(request.Id);

            if (currentProjectGroup == null)
                return Json(new { success = false, ErrorMessage = "ProjectGroup not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the ProjectGroup to swap with (higher for move down, lower for move up)
            var swapProjectGroup = (await _projectGroupService.GetAll())
                .Where(pg => isMoveUp ? pg.SortOrder < currentProjectGroup.SortOrder : pg.SortOrder > currentProjectGroup.SortOrder)
                .OrderBy(pg => isMoveUp ? pg.SortOrder * -1 : pg.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapProjectGroup == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No ProjectGroup to move up." : "No ProjectGroup to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentProjectGroup.SortOrder;

            currentProjectGroup.SortOrder = swapProjectGroup.SortOrder;

            swapProjectGroup.SortOrder = tempSortOrder;

            // Update both records
            await _projectGroupService.Update(currentProjectGroup);

            await _projectGroupService.Update(swapProjectGroup);

            return Json(new { success = true });
        }
    }
}