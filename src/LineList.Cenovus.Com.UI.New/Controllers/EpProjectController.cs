using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.EpProject;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.RulesEngine;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.UI.New.Controllers
{
    public class EpProjectController : Controller
    {
        private readonly IEpProjectService _epProjectService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;
        private readonly ILineListStatusService _lineListStatusService;
        private readonly IEpCompanyAlphaService _epCompanyAlphaService;
        private readonly ISpecificationService _specificationService;
        private readonly ILineListModelService _lineListModelService;
        private readonly ILineListRevisionService _lineListRevisionService;
        private readonly IEpProjectInsulationDefaultService _epProjectInsulationDefaultService;
        private readonly IInsulationDefaultRowService _insulationDefaultRowService;
        private readonly IEpProjectInsulationDefaultRowService _epProjectInsulationDefaultRowService;
        private readonly IInsulationDefaultColumnService _insulationDefaultColumnService;
        private readonly IEpProjectInsulationDefaultColumnService _epProjectInsulationDefaultColumnService;
        private readonly IInsulationDefaultDetailService _insulationDefaultDetailService;
        private readonly IEpProjectInsulationDefaultDetailService _epProjectInsulationDefaultDetailService;
        private readonly ILineRevisionService _lineRevisionService;
        private readonly ILineListModelService _lineListService;

        private readonly IFacilityService _facilityService;
        private readonly IProjectTypeService _projectTypeService;
        private readonly ICenovusProjectService _cenovusProjectService;
        private readonly IEpCompanyService _epCompanyService;
        private readonly IInsulationDefaultService _insulationDefaultService;
        private readonly IEpProjectUserRoleService _epProjectUserRoleService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public EpProjectController(IEpProjectService epProjectService, IMapper mapper, IFacilityService facilityService, IProjectTypeService projectTypeService, ICenovusProjectService cenovusProjectService, IEpCompanyService epCompanyService, IInsulationDefaultService insulationDefaultService, IEpProjectUserRoleService epProjectUserRoleService,
            CurrentUser currentUser,
            ILineListStatusService lineListStatusService,
            IEpCompanyAlphaService epCompanyAlphaService,
            ISpecificationService specificationService,
            ILineListModelService lineListModelService,
            ILineListRevisionService lineListRevisionService,
            IEpProjectInsulationDefaultService epProjectInsulationDefaultService,
            IInsulationDefaultRowService insulationDefaultRowService,
            IEpProjectInsulationDefaultRowService epProjectInsulationDefaultRowService,
            IInsulationDefaultColumnService insulationDefaultColumnService,
            IEpProjectInsulationDefaultColumnService epProjectInsulationDefaultColumnService,
            IInsulationDefaultDetailService insulationDefaultDetailService,
            IEpProjectInsulationDefaultDetailService epProjectInsulationDefaultDetailService,
            ILineRevisionService lineRevisionService,
            ILineListModelService lineListService,
            IConfiguration configuration,
            IWebHostEnvironment env
            )
        {
            _epProjectService = epProjectService;
            _mapper = mapper;
            _facilityService = facilityService;
            _projectTypeService = projectTypeService;
            _epCompanyService = epCompanyService;
            _cenovusProjectService = cenovusProjectService;
            _insulationDefaultService = insulationDefaultService;
            _epProjectUserRoleService = epProjectUserRoleService;
            _currentUser = currentUser;

            _lineListStatusService = lineListStatusService;
            _epCompanyAlphaService = epCompanyAlphaService;
            _specificationService = specificationService;
            _lineListModelService = lineListModelService;
            _lineListRevisionService = lineListRevisionService;
            _epProjectInsulationDefaultService = epProjectInsulationDefaultService;
            _insulationDefaultRowService = insulationDefaultRowService;
            _epProjectInsulationDefaultRowService = epProjectInsulationDefaultRowService;
            _insulationDefaultColumnService = insulationDefaultColumnService;
            _epProjectInsulationDefaultColumnService = epProjectInsulationDefaultColumnService;
            _insulationDefaultDetailService = insulationDefaultDetailService;
            _epProjectInsulationDefaultDetailService = epProjectInsulationDefaultDetailService;
            _lineRevisionService = lineRevisionService;
            _lineListService = lineListService;
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index(bool? showActive)
        {
            ViewData["IsCenovusAdmin"] = _currentUser.IsCenovusAdmin;
            ViewData["IsEpAdmin"] = _currentUser.IsEpAdmin;
            ViewData["IsReadOnly"] = _currentUser.IsReadOnly;
            ViewData["EppLeadEng"] = _currentUser.EppLeadEng.ToList();
            ViewData["EppDataEnt"] = _currentUser.EppDataEnt.ToList();
            ViewData["EppRsv"] = _currentUser.EppRsv.ToList();


            bool isActive = showActive ?? false;
            ViewData["ShowActive"] = isActive;
            var epProjects = await GetDataSource(isActive);
            var projectDtos = _mapper.Map<IEnumerable<EpProjectResultDto>>(epProjects);
            return View(projectDtos);
        }

        [HttpGet]
        public async Task<JsonResult> EpProjectFeed()
        {
            var projects = await _epProjectService.GetAll();
            var projectDtos = _mapper.Map<IEnumerable<EpProjectResultDto>>(projects);
            return Json(new { data = projectDtos });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["IsCenovusAdmin"] = _currentUser.IsCenovusAdmin;
            ViewData["IsEpAdmin"] = _currentUser.IsEpAdmin;
            ViewData["IsReadOnly"] = _currentUser.IsReadOnly;
            ViewData["EppLeadEng"] = _currentUser.EppLeadEng.ToList();
            ViewData["EppDataEnt"] = _currentUser.EppDataEnt.ToList();
            ViewData["EppRsv"] = _currentUser.EppRsv.ToList();


            var facilities = (await _facilityService.GetAll()).Where(s=>s.IsActive).OrderBy(s=>s.SortOrder).ToList();
            var projectTypes = _projectTypeService.GetAll().Result.Where(s => s.IsActive).OrderBy(s => s.SortOrder).ToList();
            var cenovusProjects = _cenovusProjectService.GetAll().Result.Where(s => s.IsActive).OrderBy(s => s.SortOrder).ToList();

            var insulationDefaults = _insulationDefaultService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var projectUserRoles = new List<EpProjectUserRole>();
            EpProject newProject = new EpProject()
            {
                CreatedBy = _currentUser.FullName,
                CreatedOn = DateTime.Now,
                ModifiedBy = _currentUser.FullName,
                ModifiedOn = DateTime.Now,
                IsActive = true,
                EpCompanyId = _currentUser.EpCompanyId.HasValue ? _currentUser.EpCompanyId.Value : Guid.Empty
            };

            var canChangeEp = _currentUser.IsCenovusAdmin || (_currentUser.IsIODFieldFCAdmUser || _currentUser.IsIODFieldCLAdmUser);


            var canAddRole = _currentUser.IsCenovusAdmin || _currentUser.EpAdmin.Contains(newProject.EpCompanyId);
            var canSave = _currentUser.IsCenovusAdmin || (_currentUser.EpAdmin.Contains(newProject.EpCompanyId) && newProject.Id == Guid.Empty) || _currentUser.EppLeadEng.Contains(newProject.EpCompanyId) || _currentUser.IsIODFieldCLAdmUser || _currentUser.IsIODFieldFCAdmUser;
            EPProjectViewModel ePProjectViewModel = new EPProjectViewModel()
            {
                EpProjectAdd = _mapper.Map<EpProjectAddDto>(newProject),
                Facilities = facilities,
                ProjectTypes = projectTypes,
                CenovusProjects = cenovusProjects,
                EpCompanies = GetEpValues(),
                EpProjectUserRoles = projectUserRoles,
                InsulationDefaults = insulationDefaults,
                InsulationDefaultsValues = GetInsulationDefaults(null),
                IsEpAdmin = _currentUser.IsEpAdmin && !_currentUser.IsCenovusAdmin,
                CanAddRole = canAddRole,
                CanSave = canSave,
                CanChangeEp = !canChangeEp
            };
            //// If this user is an EP Admin (but not a Cenovus Admin), lock EP to their company
            // if (_currentUser.IsEpAdmin && !_currentUser.IsCenovusAdmin)
            // {
            //    ePProjectViewModel.EpProjectAdd.EpCompanyId = _currentUser.EpCompanyId ?? Guid.Empty;
            //    ViewData["DisableEpSelect"] = true;
            // }
            return PartialView("_Create", ePProjectViewModel);
        }

        private List<EpCompany> GetEpValues()
        {
            var epCompanies = _epCompanyService.GetAll().Result.OrderBy(s => s.SortOrder).Where(e => e.IsActive).ToList();
            if (_currentUser.IsIODFieldCLAdmUser && _currentUser.IsIODFieldFCAdmUser)
            {
                epCompanies = epCompanies.Where(m => m.Name == "CVE-FIELD-CL" || m.Name == "CVE-FIELD-FC").ToList();
            }
            else
            {
                if (_currentUser.IsIODFieldCLAdmUser)
                {
                    epCompanies = epCompanies.Where(m => m.Name == "CVE-FIELD-CL").ToList();
                }
                if (_currentUser.IsIODFieldFCAdmUser)
                {
                    epCompanies = epCompanies.Where(m => m.Name == "CVE-FIELD-FC").ToList();
                }
            }
            return epCompanies;
        }

        public JsonResult GetInsulationDefaultsValues(string epCompanyId)
        {
            return Json(GetInsulationDefaults(epCompanyId));
        }

        private Dictionary<Guid, string> GetInsulationDefaults(string epCompanyId)
        {
            Dictionary<Guid, string> list = new Dictionary<Guid, string>();
            List<EpProject> epCos;

            var data = _epProjectService.GetAll().Result.OrderBy(s => s.SortOrder).Where(m => true);

            if (!string.IsNullOrWhiteSpace(epCompanyId))
                data = data.Where(m => m.EpCompanyId == new Guid(epCompanyId));

            data = data.Where(m => m.IsActive == true);

            epCos = data.OrderBy(m => m.Name).ToList();

            foreach (EpProject ep in epCos)
                if (!list.ContainsKey(ep.Id))
                    list.Add(ep.Id, ep.Name_dash_Description);

            return list;
        }

        [HttpPost]
        public async Task<JsonResult> Create(EPProjectViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return Json(new { success = false, ErrorMessage = "Model is not valid" });

            try
            {
                var project = _mapper.Map<EpProject>(model.EpProjectAdd);
                project.Id = Guid.NewGuid();
                project.CreatedBy = _currentUser.FullName;
                project.CreatedOn = DateTime.Now;
                project.ModifiedBy = _currentUser.FullName;
                project.ModifiedOn = DateTime.Now;
                project.IsActive = true;
                project.CenovusProjectId = null;// model.SelectedProjectTypeId;
                                                // project.CenovusProject.ProjectTypeId= model.SelectedProjectTypeId.Value;

                var allProjects = await _epProjectService.GetAll();
                if (allProjects.Any(p => p.Name.Equals(project.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    return Json(new { success = false, errorMessage = "NameNotUnique: The Project name entered already exists!" });
                }

                // Validation for other conditions (like EP Alpha)
                if (project.EpCompanyId == Guid.Empty)
                {
                    return Json(new { success = false, errorMessage = "No EP assigned: The selected facility does not have an EP assigned." });
                }

                // Add the project
                await _epProjectService.Add(project);

                // Create Reserved Line List
                await EpProjectRules.CreateReservedLineList(
                    project,
                    _currentUser.Username,
                    _lineListStatusService,
                    _cenovusProjectService,
                    _epCompanyAlphaService,
                    _specificationService,
                    _lineListModelService,
                    _lineListRevisionService
                );

                await Task.Run(() =>
                {
                    EpProjectRules.CopyInsulationTableDefaults(project, true);

                });



                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message, LineStackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public async Task<JsonResult> EPAlphaAvailableForFacility(Guid epCompanyId, Guid facilityId)
        {
            var exists = (await _epCompanyAlphaService.GetAll())
                         .Any(a => a.EpCompanyId == epCompanyId && a.FacilityId == facilityId);
            return Json(new { valid = exists });
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {

            ViewData["IsCenovusAdmin"] = _currentUser.IsCenovusAdmin;
            ViewData["IsEpAdmin"] = _currentUser.IsEpAdmin;
            ViewData["IsReadOnly"] = _currentUser.IsReadOnly;
            ViewData["EppLeadEng"] = _currentUser.EppLeadEng.Contains(id);
            ViewData["EppDataEnt"] = _currentUser.EppDataEnt.Contains(id);
            ViewData["EppRsv"] = _currentUser.EppRsv.Contains(id);

            var project = await _epProjectService.GetById(id);
            if (project == null)
                return NotFound();

            var facilities = (await _facilityService.GetAll()).Where(s => s.IsActive).OrderBy(s => s.SortOrder).ToList();
            var projectTypes = (await _projectTypeService.GetAll()).Where(s => s.IsActive).OrderBy(s => s.SortOrder).ToList();
            var cenovusProjects = (await _cenovusProjectService.GetAll()).Where(s => s.IsActive).OrderBy(s => s.SortOrder).ToList();
            var epCompanies = (await _epCompanyService.GetAll()).Where(s => s.IsActive).OrderBy(s => s.SortOrder).ToList();
            var insulationDefaults = (await _insulationDefaultService.GetAll())
                                         .Where(o => o.IsActive)
                                         .OrderBy(o => o.SortOrder)
                                         .ToList();
            var epProjectInsulationDefaults = _epProjectInsulationDefaultService.GetAll().Result.Where(i => i.EpProjectId == project.Id && i.IsActive).OrderBy(i => i.SortOrder).ToList();
            var projectUserRoles = (await _epProjectUserRoleService.GetAll())
                                         .Where(r => r.EpProjectId == project.Id)
                                         .ToList();
            Guid? selectedType = project.CenovusProject?.ProjectTypeId;


            var canAddRole = _currentUser.IsCenovusAdmin || _currentUser.EpAdmin.Contains(project.EpCompanyId);
            var canSave = _currentUser.IsCenovusAdmin || (_currentUser.EpAdmin.Contains(project.EpCompanyId) && project.Id == Guid.Empty) || _currentUser.EppLeadEng.Contains(project.EpCompanyId) || _currentUser.IsIODFieldCLAdmUser || _currentUser.IsIODFieldFCAdmUser;
            var canChangeCenovusProject = true;
            var canChangeFacility = true;
            var canChangeDescription = true;
            if (!EpProjectRules.CanChangeFacility(project, _lineRevisionService).Result)
            {
                canChangeCenovusProject = false;
                canChangeFacility = false;
            }
            if (!_currentUser.IsCenovusAdmin && !_currentUser.EppLeadEng.Contains(project.Id))
            {
                //lock everything
                canChangeCenovusProject = false;
                canChangeDescription = false;
            }

            var canBeTurnover = await EpProjectRules.CanBeTurnedOver(id, _lineRevisionService, _lineListRevisionService);

            var canTurnover = _currentUser.IsCenovusAdmin || _currentUser.EpAdmin.Contains(project.EpCompanyId);
            project.ProjectTypeId = (project.ProjectTypeId.HasValue && project.ProjectTypeId.Value != Guid.Empty)
       ? project.ProjectTypeId.Value
       : project.CenovusProject.ProjectTypeId;
            project.FacilityId = (project.FacilityId.HasValue && project.FacilityId.Value != Guid.Empty)
       ? project.FacilityId.Value
       : project.CenovusProject.FacilityId;
            var vm = new EPProjectViewModel
            {
                EpProject = _mapper.Map<EpProjectEditDto>(project),
                Facilities = facilities,
                ProjectTypes = projectTypes,
                CenovusProjects = cenovusProjects,
                EpCompanies = epCompanies,
                EpProjectUserRoles = projectUserRoles,
                EpProjectInsulationDefaults = epProjectInsulationDefaults,
                InsulationDefaultsValues = GetInsulationDefaults(project.EpCompanyId.ToString()),
                CanDelete = _currentUser.IsCenovusAdmin || _currentUser.EppLeadEng.Contains(project.Id),
                CanAddRole = canAddRole,
                CanSave = canSave,
                CanChangeCenovusProject = canChangeCenovusProject,
                CanChangeFacility = canChangeFacility,
                CanChangeDescription = canChangeDescription,
                CanBeTurnedOver = canBeTurnover,
                CanTurnover = canBeTurnover
            
            };
            //vm.EpProject.ProjectTypeId = project.CenovusProject?.ProjectTypeId;

            return PartialView("_Update", vm);
        }

        [HttpPost]
        public async Task<JsonResult> Update(EPProjectViewModel model)
        {
            var epProject = await _epProjectService.GetById(model.EpProject.Id);

            epProject.ProjectTypeId = model.EpProject.ProjectTypeId;
            epProject.Description = model.EpProject.Description;
            epProject.IsActive = model.EpProject.IsActive;
            epProject.ModifiedBy = _currentUser.FullName;
            epProject.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
            await _epProjectService.Update(epProject);

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Guid id)
        {
            try
            {
                bool success = await EpProjectRules.Delete(
                                                    id,
                                                    _epProjectService,
                                                    _epProjectUserRoleService,
                                                    _lineListRevisionService,
                                                    _epProjectInsulationDefaultDetailService,
                                                    _epProjectInsulationDefaultColumnService,
                                                    _epProjectInsulationDefaultRowService,
                                                    _epProjectInsulationDefaultService,
                                                    _lineListService
                                                );
                return Json(new { success });
            }

            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }

        }
        [HttpPost]
        public async Task<JsonResult> MoveSortOrder([FromBody] MoveSortOrderRequest request)
        {
            if (request.Id == Guid.Empty || string.IsNullOrEmpty(request.Direction))
                return Json(new { success = false, ErrorMessage = "Invalid request data" });

            var currentItem = await _epProjectInsulationDefaultService.GetById(request.Id);

            if (currentItem == null)
                return Json(new { success = false, ErrorMessage = "Item not found" });

            bool isMoveUp = request.Direction.ToLower() == "up";

            var swapItem = (await _epProjectInsulationDefaultService.GetAll())
                .Where(p => isMoveUp ? p.SortOrder < currentItem.SortOrder : p.SortOrder > currentItem.SortOrder)
                .OrderBy(p => isMoveUp ? p.SortOrder * -1 : p.SortOrder) // Desc for up, Asc for down
                .FirstOrDefault();

            if (swapItem == null)
                return Json(new { success = false, ErrorMessage = isMoveUp ? "No item to move up." : "No item to move down." });

            int tempSortOrder = currentItem.SortOrder;

            currentItem.SortOrder = swapItem.SortOrder;
            swapItem.SortOrder = tempSortOrder;

            await _epProjectInsulationDefaultService.Update(currentItem);
            await _epProjectInsulationDefaultService.Update(swapItem);

            return Json(new { success = true });
        }


        [HttpGet]
        public async Task<IActionResult> Turnover(Guid id)
        {
            var project = await _epProjectService.GetById(id);
            if (project == null)
                return NotFound();

            if (!_currentUser.IsCenovusAdmin && !_currentUser.EpAdmin.Contains(project.EpCompanyId))
                return Unauthorized();

            var epCompanies = _epCompanyService.GetAll().Result.Where(e => e.IsActive).ToList();
            var facilityId = project.CenovusProject?.FacilityId ?? project.FacilityId;
            var availableEpCompanies = (await _epCompanyAlphaService.GetAll())
                .Where(a => a.FacilityId == facilityId)
                .Select(a => a.EpCompanyId)
                .Distinct()
                .Join(epCompanies, id => id, ep => ep.Id, (id, ep) => ep)
                .ToList();

            var defaultToEpCompanyId = availableEpCompanies
                .FirstOrDefault(c => c.Name.Contains("Cenovus", StringComparison.OrdinalIgnoreCase))?.Id;

            var viewModel = new TurnoverViewModel
            {
                ProjectId = id,
                ProjectName = project.Name,
                FromEpCompanyId = project.EpCompanyId,
                FromEpCompanyName = project.EpCompany.Name + " - " + project.EpCompany.Description,
                ToEpCompanyId = defaultToEpCompanyId ?? availableEpCompanies.FirstOrDefault().Id,
                EpCompanies = epCompanies,
                IsCenovusAdmin = _currentUser.IsCenovusAdmin
            };

            return PartialView("_Turnover", viewModel);
        }

        [HttpPost]
        public async Task<JsonResult> Turnover(TurnoverViewModel model)
        {
            try
            {
                var project = await _epProjectService.GetById(model.ProjectId);
                if (project == null)
                    return Json(new { success = false, errorMessage = "Project not found." });

                if (!_currentUser.IsCenovusAdmin && !_currentUser.EpAdmin.Contains(project.EpCompanyId))
                    return Json(new { success = false, errorMessage = "Unauthorized." });

                // Await the Task<bool> result before applying the '!' operator
                if (!await EpProjectRules.CanBeTurnedOver(model.ProjectId, _lineRevisionService, _lineListRevisionService))
                    return Json(new { success = false, errorMessage = "Project cannot be turned over due to draft line lists or reserved lines." });
                var logoPath1 = Path.Combine(_env.WebRootPath, "img", "Logo_cenovus.png");
                var logoPath = _configuration.GetValue<string>("LogoPath") ?? logoPath1;

                bool ok = await EpProjectRules.TurnOver(
                    model.ProjectId,
                    model.ToEpCompanyId,
                    _currentUser,
                    logoPath,
                    _configuration
                );

                return ok
                 ? Json(new { success = true })
                 : Json(new { success = false, errorMessage = "Target EP not authorized." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

        private async Task<List<EpProjectResultDto>> GetDataSource(bool showActive)
        {
            var results = (await _epProjectService.GetDataSource(showActive));

            Guid t = (await _epCompanyService.Search("CVE-FIELD-CL")).FirstOrDefault().Id;
            Guid y = (await _epCompanyService.Search("CVE-FIELD-FC")).FirstOrDefault().Id;

            if (_currentUser.IsIODFieldCLAdmUser && _currentUser.IsIODFieldFCAdmUser)
                results = results.Where(m => m.EpCompanyName == "CVE-FIELD-CL" || m.EpCompanyName == "CVE-FIELD-FC");
            else if (_currentUser.IsIODFieldCLAdmUser)
                results = results.Where(m => m.EpCompanyName == "CVE-FIELD-CL");
            else if (_currentUser.IsIODFieldFCAdmUser)
                results = results.Where(m => m.EpCompanyName == "CVE-FIELD-FC");

            else if (_currentUser.IsIODFieldFCUser && _currentUser.IsIODFieldCLUser)
            {
                results = results.Where(m => _currentUser.EpAdmin.Contains(t) || _currentUser.EpUser.Contains(t) ||
                    _currentUser.EpAdmin.Contains(y) || _currentUser.EpUser.Contains(y));
            }
            else if (!_currentUser.IsCenovusAdmin)
                results = results.Where(m => _currentUser.EpAdmin.Contains(m.EpCompanyId) || _currentUser.EpUser.Contains(m.EpCompanyId));

            return results.ToList();
        }
    }
}