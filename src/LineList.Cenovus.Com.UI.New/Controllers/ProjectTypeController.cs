using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.ProjectType;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    public class ProjectTypeController : Controller
    {
        private readonly IProjectTypeService _projectTypeService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public ProjectTypeController(IProjectTypeService projectTypeService, IMapper mapper, CurrentUser currentUser)
        {
            _projectTypeService = projectTypeService ?? throw new ArgumentNullException(nameof(projectTypeService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _currentUser = currentUser;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var projectTypes = await _projectTypeService.GetAll();
            var projectTypeDtos = _mapper.Map<IEnumerable<ProjectTypeResultDto>>(projectTypes);
            return View(projectTypeDtos);
        }

        [HttpGet]
        public async Task<JsonResult> ProjectTypeFeed()
        {
            var projectTypes = await _projectTypeService.GetAll();
            var projectTypeDtos = _mapper.Map<IEnumerable<ProjectTypeResultDto>>(projectTypes);
            return Json(new { data = projectTypeDtos });
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ProjectTypeAddDto
            {
                CreatedBy = _currentUser.FullName,
                ModifiedBy = _currentUser.FullName
            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(ProjectTypeAddDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var projectType = _mapper.Map<ProjectType>(model);
           var newProjectType= await _projectTypeService.Add(projectType);

            if (newProjectType == null)
            {
                return Json(new { success = false, ErrorMessage = "<b>Duplicate Name</b> : The value entered in name field already exists!" });
            }


            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var projectType = await _projectTypeService.GetById(id);
            if (projectType == null)
                return NotFound();

            var canDel = true;
            string message = "";
            if (_projectTypeService.HasDependencies(id))
            {
                message = string.Format("Cannot Delete: \r\n\r\n{0}: {1} is currently referenced by an existing Project Group / Expansion Phase", "Project Type", projectType.Name);
                message += " and cannot be deleted.\r\n\r\nPlease consider using the Edit function to uncheck the Active indicator instead.";

                canDel = false;
            }
            ViewData["CanDel"] = canDel;
            ViewData["Message"] = message;

            var projectTypeDto = _mapper.Map<ProjectTypeEditDto>(projectType);
            return PartialView("_Update", projectTypeDto);
        }

        [HttpPost]
        public async Task<JsonResult> Update(ProjectTypeEditDto model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            model.ModifiedBy = _currentUser.FullName;
            model.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            var projectType = _mapper.Map<ProjectType>(model);
            await _projectTypeService.Update(projectType);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            var projectType = await _projectTypeService.GetById(id);
            if (projectType == null)
                return Json(new { success = false, ErrorMessage = "ProjectType not found" });           

            await _projectTypeService.Remove(projectType);
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentProjectType = await _projectTypeService.GetById(request.Id);

            if (currentProjectType == null)
                return Json(new { success = false, ErrorMessage = "ProjectType not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            // Find the ProjectType to swap with (higher for move down, lower for move up)
            var swapProjectType = (await _projectTypeService.GetAll())
                .Where(pt => isMoveUp ? pt.SortOrder < currentProjectType.SortOrder : pt.SortOrder > currentProjectType.SortOrder)
                .OrderBy(pt => isMoveUp ? pt.SortOrder * -1 : pt.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapProjectType == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No ProjectType to move up." : "No ProjectType to move down." });

            // Swap SortOrder values
            int tempSortOrder = currentProjectType.SortOrder;

            currentProjectType.SortOrder = swapProjectType.SortOrder;

            swapProjectType.SortOrder = tempSortOrder;

            // Update both records
            await _projectTypeService.Update(currentProjectType);

            await _projectTypeService.Update(swapProjectType);

            return Json(new { success = true });
        }
    }
}