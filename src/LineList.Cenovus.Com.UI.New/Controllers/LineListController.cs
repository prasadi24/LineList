using AutoMapper;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Excel;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Services;
using LineList.Cenovus.Com.RulesEngine;
using LineList.Cenovus.Com.Security;
using LineList.Cenovus.Com.UI.New.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.IO;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using Rotativa.AspNetCore;
using System.Linq;
using System.Threading.Tasks;
using Path = System.IO.Path;
using Table = MigraDoc.DocumentObjectModel.Tables.Table;



namespace LineList.Cenovus.Com.UI.New.Controllers
{
    public class LineListController : Controller
    {
        private readonly IFacilityService _facilityService;
        private readonly ISpecificationService _specificationService;
        private readonly ILocationService _locationService;
        private readonly IAreaService _areaService;
        private readonly ICenovusProjectService _cenovusProjectService;
        private readonly IEpCompanyService _epCompanyService;
        private readonly IEpProjectService _epProjectService;
        private readonly IProjectTypeService _projectTypeService;
        private readonly ILineListStatusService _lineListStatusService;
        private readonly ILineListModelService _lineListModelService;
        private readonly IModularService _modularService;
        private readonly ILineRevisionService _lineRevisionService;
        private readonly ILineService _lineService;
        private readonly ILineListRevisionService _lineListRevisionService;
        private readonly ILineRevisionOperatingModeService _lineRevisionOperatingModeService;
        private readonly ILineRevisionSegmentService _lineRevisionSegmentService;
        private readonly IMapper _mapper;
        private readonly LineListRules _lineListRules;
        private readonly CurrentUser _currentUser;
        private readonly IWebHostEnvironment _env;
        public LineListController(
            IFacilityService facilityService,
            ISpecificationService specificationService,
            ILocationService locationService,
            IAreaService areaService,
            ICenovusProjectService cenovusProjectService,
            IEpCompanyService epCompanyService,
            IEpProjectService epProjectService,
            IProjectTypeService projectTypeService,
            ILineListStatusService lineListStatusService,
            ILineListModelService lineListModelService,
            IModularService modularService,
            ILineListRevisionService lineListRevisionService,
            ILineRevisionService lineRevisionService,
            ILineService lineService,
            ILineRevisionOperatingModeService lineRevisionOperatingModeService,
            ILineRevisionSegmentService lineRevisionSegmentService,
            LineListRules lineListRules,
            IMapper mapper,
            CurrentUser currentUser,
             IWebHostEnvironment env)
        {
            _facilityService = facilityService;
            _specificationService = specificationService;
            _locationService = locationService;
            _areaService = areaService;
            _cenovusProjectService = cenovusProjectService;
            _epCompanyService = epCompanyService;
            _epProjectService = epProjectService;
            _projectTypeService = projectTypeService;
            _lineListStatusService = lineListStatusService;
            _lineListModelService = lineListModelService;
            _modularService = modularService;
            _lineListRevisionService = lineListRevisionService;
            _lineRevisionService = lineRevisionService;
            _lineService = lineService;
            _lineRevisionOperatingModeService = lineRevisionOperatingModeService;
            _lineRevisionSegmentService = lineRevisionSegmentService;
            _mapper = mapper;
            _lineListRules = lineListRules;
            _currentUser = currentUser;
            _env = env;

        }

        public async Task<IActionResult> Index(Guid? EpProject = null, string DocumentNumberId = null, Guid? FacilityId = null)
        {
            var facilities = _facilityService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var cenovusProjects = _cenovusProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var projectTypes = _projectTypeService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var specifications = _specificationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var ePs = _epCompanyService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineListStatuses = _lineListStatusService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var locations = _locationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var ePProjects = _epProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var areas = _areaService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineLists = _lineListModelService.GetAll().Result.ToList();
            var docNumbers = GetDocumentNumber(FacilityId.HasValue ? FacilityId.Value : Guid.Empty, Guid.Empty, Guid.Empty, EpProject.HasValue ? EpProject.Value : Guid.Empty, Guid.Empty);
            // Initialize ModularDetails as an empty list by default
            List<ModularDropDownModel> modularDetails = new List<ModularDropDownModel>();

            var canAdd = _currentUser.IsCenovusAdmin || _currentUser.EppLeadEng.Length > 0 || _currentUser.EppDataEnt.Length > 0;

            var model = new SearchLineListViewModel
            {
                Facilities = facilities,
                CenovusProjects = cenovusProjects,
                ProjectTypes = projectTypes,
                Specifications = specifications,
                EPs = ePs,
                LineListStatuses = lineListStatuses,
                Locations = locations,
                EPProjects = ePProjects,
                Areas = areas,
                LineLists = lineLists,
                DocNumbers = docNumbers,
                ModularDetails = modularDetails,
                ShowOnlyActive = true,
                ShowDrafts = true,
                Description = "",
                CanAdd = canAdd
            };
            if (EpProject.HasValue)
            {
                Console.WriteLine($"EpProject ID: {EpProject.Value}");
                var epProject = await _epProjectService.GetById(EpProject.Value);
                if (epProject != null)
                {
                    Console.WriteLine("EpProject found, setting AutoSearch to true.");
                    model.SelectedFacilityId = epProject.CenovusProject?.FacilityId ?? Guid.Empty;
                    model.SelectedEPId = epProject.EpCompanyId;
                    model.SelectedEPProjectId = epProject.Id;
                    model.SelectedProjectId = epProject.CenovusProjectId ?? Guid.Empty;
                    model.SelectedProjectTypeId = epProject.CenovusProject?.ProjectTypeId ?? Guid.Empty;
                    // Fetch ModularDetails using the selected values
                    model.ModularDetails = await _modularService.GetModularDetails(
                    model.SelectedFacilityId,
                    model.SelectedProjectTypeId,
                    model.SelectedEPId,
                    model.SelectedEPProjectId,
                    model.SelectedProjectId

                    );
                    model.AutoSearch = true;
                }
                else
                {
                    ViewData["ErrorMessage"] = "Project not found.";
                }
            }
            // Handle Redo Search query parameters
            if (!string.IsNullOrEmpty(DocumentNumberId) && FacilityId.HasValue)
            {
                model.SelectedDocumentNumberId = DocumentNumberId;
                model.SelectedFacilityId = FacilityId.Value;
                model.AutoSearch = true; // Trigger auto-search to refresh the grid
            }
            return View(model);
        }

        // New action to fetch ModularDetails dynamically based on selected filters
        [HttpGet]
        public async Task<IActionResult> GetModularDetails(Guid facilityId, Guid projectTypeId, Guid epCompanyId, Guid epProjectId, Guid cenovusProjectId)
        {
            var modularDetails = await _modularService.GetModularDetails(facilityId, projectTypeId, epCompanyId, epProjectId, cenovusProjectId);
            return Json(modularDetails);
        }
        public async Task<IActionResult> SearchResult(SearchLineListViewModel model)
        {
            var results = await _lineListRevisionService.GetFilteredLineListRevisions(
                model.SelectedFacilityId != Guid.Empty ? model.SelectedFacilityId : (Guid?)null,
                model.LineListId != Guid.Empty ? model.LineListId : (Guid?)null,
                model.SelectedLocationId != Guid.Empty ? model.SelectedLocationId : (Guid?)null,
                model.SelectedAreaId != Guid.Empty ? model.SelectedAreaId : (Guid?)null,
                model.SelectedEPId != Guid.Empty ? model.SelectedEPId : (Guid?)null,
                model.SelectedProjectId != Guid.Empty ? model.SelectedProjectId : (Guid?)null,
                model.SelectedEPProjectId != Guid.Empty ? model.SelectedEPProjectId : (Guid?)null,
                model.SelectedLineListStatusId != Guid.Empty ? model.SelectedLineListStatusId : (Guid?)null,
                model.ShowDrafts,
                model.ShowOnlyActive,
                model.SelectedDocumentNumberId,
                model.SelectedModularID,
                model.SelectedProjectTypeId != Guid.Empty ? model.SelectedProjectTypeId : (Guid?)null
            );

            foreach (var dto in results)
            {
                dto.CanEdit = _currentUser.IsCenovusAdmin || _currentUser.EppLeadEng.Contains(dto.EpProjectId)
                                || _currentUser.EppDataEnt.Contains(dto.EpProjectId);

                dto.CanBeUpReved = dto.HasLines && dto.IsIssued && dto.IsHighestRev
                                    && dto.LineListStatusName?.ToUpperInvariant() != "CANCELLED"
                                    && !dto.HasActiveDrafts;
            }
            return Json(results);
        }

        [HttpPost]
        public async Task<IActionResult> RedoSearch(Guid id)
        {
            try
            {
                // Validate the ID
                if (id == Guid.Empty)
                {
                    return Json(new { success = false, errorMessage = "Invalid line list revision ID." });
                }

                // Retrieve the LineListRevision
                var lineListRevision = await _lineListRevisionService.GetById(id);
                if (lineListRevision == null)
                {
                    return Json(new { success = false, errorMessage = "Line list revision not found." });
                }

                // Ensure the DocumentNumber and FacilityId exist
                if (lineListRevision.LineList == null || string.IsNullOrWhiteSpace(lineListRevision.LineList.DocumentNumber))
                {
                    return Json(new { success = false, errorMessage = "Document number not found for this revision." });
                }

                if (lineListRevision.EpProject?.CenovusProject?.FacilityId == null)
                {
                    return Json(new { success = false, errorMessage = "Facility not found for this revision." });
                }

                // Get the DocumentNumberId (LineList.Id) and FacilityId
                var documentNumberId = lineListRevision.LineList.Id;
                var facilityId = lineListRevision.EpProject.CenovusProject.FacilityId;

                // Construct the redirect URL with query parameters
                var redirectUrl = Url.Action("Index", "LineList", new
                {
                    DocumentNumberId = documentNumberId,
                    FacilityId = facilityId
                });

                return Json(new { success = true, redirectUrl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"Error performing redo search: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var facilities = _facilityService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var cenovusProjects = _cenovusProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var projectTypes = _projectTypeService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var specifications = _specificationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var ePs = _epCompanyService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineListStatuses = LineListRules.GetValidStatusForNew();
            var locations = _locationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var ePProjects = _epProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var areas = _areaService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineLists = _lineListModelService.GetAll().Result.ToList();

            var model = new SearchLineListViewModel
            {
                Facilities = facilities,
                CenovusProjects = cenovusProjects,
                ProjectTypes = projectTypes,
                Specifications = specifications,
                EPs = ePs,
                LineListStatuses = lineListStatuses,
                Locations = locations,
                EPProjects = ePProjects,
                Areas = areas,
                LineLists = lineLists,
                CreatedBy = "User",
                ModifiedBy = "User",

            };
            return PartialView("_Create", model);
        }

        [HttpPost]
        public async Task<JsonResult> Create(SearchLineListViewModel model)
        {
            Guid tempGuid = Guid.Empty;
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                return Json(new { success = false, ErrorMessage = "Model is not valid" });
            }

            var lineListModel = new LineListModel()
            {
                Id = Guid.NewGuid(),
                CreatedBy = _currentUser.FullName,
                CreatedOn = DateTime.Now,
                DocumentNumber = model.DocumentNumber ?? model.DocumentNumber.Trim(),

                ModifiedBy = _currentUser.FullName,
                ModifiedOn = DateTime.Now
            };



            var lineListRevision = new LineListRevision()
            {
                Id = Guid.NewGuid(),
                CreatedBy = _currentUser.FullName,
                CreatedOn = DateTime.Now,
                LineList = lineListModel,
                ModifiedBy = _currentUser.FullName,
                ModifiedOn = DateTime.Now,
                IssuedOn = DateTime.Now,
                IsActive = true,
                IsSimpleRevisionBlock = true
            };
            lineListRevision.LineListId = lineListModel.Id;

            Guid epProjectId = model.SelectedEPProjectId;
            EpProject epProj = _epProjectService.GetById(epProjectId).Result;

            lineListRevision.DocumentRevision = GetNewRevisionNumber(model.SelectedFacilityId, model.SelectedLineListStatusId, model.SelectedEPId, epProj.CenovusProject.FacilityId);

            lineListRevision.LineListStatusId = model.SelectedLineListStatusId;
            lineListRevision.LocationId = model.SelectedLocationId;
            lineListRevision.EpProjectId = model.SelectedEPProjectId;
            lineListRevision.EpCompanyId = model.SelectedEPId;
            lineListRevision.AreaId = model.SelectedAreaId;
            lineListRevision.SpecificationId = model.SelectedSpecificationId;

            if (model.SelectedBlock == "simple")
            {
                lineListRevision.ApprovedByProject = model.ApprovedByProject;
                lineListRevision.ApprovedByLead = model.ApprovedByLead;
            }
            else
            {
                lineListRevision.ApprovedByProject = model.ApprovedByProjectComplex;
                lineListRevision.ApprovedByLead = model.ApprovedByLeadComplex;
            }

            lineListRevision.PreparedByMechanical = model.PreparedByMechanical;
            lineListRevision.PreparedByProcess = model.PreparedByProcess;
            lineListRevision.ReviewedByMechanical = model.ReviewedByMechanical;
            lineListRevision.ReviewByProcess = model.ReviewByProcess;
            lineListRevision.Description = model.Description;

            lineListModel.DocumentNumber = model.DocumentNumber;


            lineListRevision.PreparedBy = model.PreparedBy;
            lineListRevision.ReviewedBy = model.ReviewedBy;

            lineListRevision.ModifiedOn = DateTime.Now;
            lineListRevision.ModifiedBy = "_currentUser.FullName";

            //var hiddenChange = await _lineListRevisionService.GetHiddenChangeById(lineListRevision.Id);
            //if (hiddenChange?.Value != GetHiddenChanges(lineListRevision))
            //{
            //    lineListRevision.ModifiedOn = DateTime.Now;
            //    lineListRevision.ModifiedBy = CurrentUser.Username;
            //}


            await _lineListModelService.Add(lineListModel);
            await _lineListRevisionService.Add(lineListRevision);
            return Json(new { success = true });
        }
        public string GetNewRevisionNumber(Guid facilityId, Guid lineListStatusId, Guid epCompanyId, Guid epProjectFacilityId)
        {
            if (lineListStatusId == Guid.Empty || epCompanyId == Guid.Empty)
            {
                return "(pending)";
            }
            else
            {
                Guid finalFacilityId;

                if (facilityId == Guid.Empty)
                {
                    finalFacilityId = epProjectFacilityId; // Use epProjectFacilityId if facilityId is empty
                }
                else
                {
                    finalFacilityId = facilityId;
                }

                if (finalFacilityId == Guid.Empty)
                {
                    return "(pending)";
                }
                else
                {
                    return _lineListRules.GetNewRevision(finalFacilityId, lineListStatusId, epCompanyId);
                }
            }
        }

        public string GetHiddenChanges(LineListRevision lineListRevision)
        {
            string value = string.Empty;

            value += lineListRevision.LineListStatusId.ToString() + ',';
            value += lineListRevision.LineListDocumentNumber + ',';
            value += lineListRevision.EpProjectId.ToString() + ',';
            value += lineListRevision.SpecificationId.ToString() + ',';
            value += lineListRevision.LocationId?.ToString() + ',';  // Handle nullable GUIDs with ?
            value += lineListRevision.AreaId?.ToString() + ',';      // Handle nullable GUIDs with ?
            value += lineListRevision.Description + ',';
            value += lineListRevision.IsActive.ToString() + ',';
            // Handle Simple Block conditions
            value += (lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ApprovedByLead : string.Empty) + ',';
            value += (lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ApprovedByProject : string.Empty) + ',';

            // Always include these fields
            value += lineListRevision.PreparedBy + ',';
            value += lineListRevision.PreparedByMechanical + ',';
            value += lineListRevision.PreparedByProcess + ',';
            value += lineListRevision.ReviewByProcess + ',';
            value += lineListRevision.ReviewedBy + ',';
            value += lineListRevision.ReviewedByMechanical + ',';

            // Handle Complex Block conditions
            value += (!lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ApprovedByLead : string.Empty) + ',';
            value += (!lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ApprovedByProject : string.Empty) + ',';

            value += lineListRevision.IsSimpleRevisionBlock.ToString() + ',';
            value += lineListRevision.IsLocked.ToString();

            return value;
        }
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var lineListRevision = await _lineListRevisionService.GetById(id);
            if (lineListRevision == null)
            {
                return NotFound();
            }

            var facilities = _facilityService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var projectTypes = _projectTypeService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var ePs = _epCompanyService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var listListValidStatuses = LineListRules.GetValidStatus(lineListRevision.Id, false, true);
            var ePProjects = _epProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();

            var isDraft = lineListRevision.LineListStatus?.IsDraftOfId != null;
            var isReserved = lineListRevision.LineListStatus?.Name?.ToUpper() == "RESERVED";
            var hasBeenIssued = LineListRules.HasBeenIssued(ref lineListRevision) || lineListRevision.LineListStatus?.IsIssuedOfId != null;

            var printLabel = (lineListRevision.LineListStatus.IsIssuedOfId != null || !isDraft) ? "PRINT FOR REFERENCE" : "PRINT FOR REVIEW";

            var mostRecentRevisionId = lineListRevision.LineList?.LineListRevisions
                .OrderByDescending(m => m.LineListStatus.IsHardRevision)
                .ThenByDescending(m => m.DocumentRevisionSort)
                .FirstOrDefault()?.Id;
            var isMostRecentRevision = mostRecentRevisionId == lineListRevision.Id;

            var isCenovusAdmin = _currentUser.IsCenovusAdmin;
            var isEpLeadEngineer = _currentUser.EppLeadEng.Contains(lineListRevision.EpProjectId);
            var isEpDataEntry = _currentUser.EppDataEnt.Contains(lineListRevision.EpProjectId);
            var isEditable = isCenovusAdmin || isEpDataEntry || isEpLeadEngineer;

            var isReadOnly = isEditable ? false : true;

            var areas = GetAreas(lineListRevision.AreaId ?? Guid.Empty, lineListRevision.EpProject?.CenovusProject?.FacilityId ?? Guid.Empty, lineListRevision.SpecificationId, lineListRevision.LocationId ?? Guid.Empty);
            var cenovusProjects = GetCenovusProjects(lineListRevision.EpProject?.CenovusProject?.Id ?? Guid.Empty, lineListRevision.EpProject?.CenovusProject?.FacilityId ?? Guid.Empty, lineListRevision.EpProject?.CenovusProjectId ?? Guid.Empty);
            var locations = GetLocations(lineListRevision.EpProject?.CenovusProject?.FacilityId ?? Guid.Empty, lineListRevision.EpProject?.CenovusProject?.Id ?? Guid.Empty, lineListRevision.LocationId ?? Guid.Empty);
            var specifications = GetSpecifications(lineListRevision.EpProject?.CenovusProject?.FacilityId ?? Guid.Empty, lineListRevision.SpecificationId);
            var epProjectId = lineListRevision.EpProjectId;
            var model = new SearchLineListViewModel
            {
                LineListRevisionId = lineListRevision.Id,
                DocumentNumber = lineListRevision.LineList?.DocumentNumber ?? string.Empty,
                DocumentRevision = lineListRevision.DocumentRevision ?? string.Empty,
                SelectedLineListStatusId = lineListRevision.LineListStatusId,
                SelectedLineListStatusName = lineListRevision.LineListStatus?.Name ?? string.Empty,
                SelectedAreaId = lineListRevision.AreaId ?? Guid.Empty,
                SelectedProjectId = lineListRevision.EpProject?.CenovusProjectId ?? Guid.Empty,
                SelectedEPId = lineListRevision.EpCompanyId,
                SelectedEPProjectId = lineListRevision.EpProjectId,
                SelectedFacilityId = lineListRevision.EpProject?.CenovusProject?.FacilityId ?? Guid.Empty,
                SelectedLocationId = lineListRevision.LocationId ?? Guid.Empty,
                SelectedProjectTypeId = lineListRevision.EpProject?.CenovusProject?.ProjectTypeId ?? Guid.Empty,
                SelectedSpecificationId = lineListRevision.SpecificationId,
                CreatedBy = lineListRevision.CreatedBy ?? string.Empty,
                CreatedOn = lineListRevision.CreatedOn,
                ModifiedBy = lineListRevision.ModifiedBy ?? string.Empty,
                ModifiedOn = lineListRevision.ModifiedOn,
                Description = lineListRevision.Description ?? string.Empty,
                IsActive = lineListRevision.IsActive,
                IsSimpleRevisionBlock = lineListRevision.IsSimpleRevisionBlock,
                PreparedBy = lineListRevision.IsSimpleRevisionBlock ? lineListRevision.PreparedBy : string.Empty,
                ReviewedBy = lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ReviewedBy : string.Empty,
                ApprovedByLead = lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ApprovedByLead : string.Empty,
                ApprovedByProject = lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ApprovedByProject : string.Empty,
                PreparedByMechanical = !lineListRevision.IsSimpleRevisionBlock ? lineListRevision.PreparedByMechanical : string.Empty,
                PreparedByProcess = !lineListRevision.IsSimpleRevisionBlock ? lineListRevision.PreparedByProcess : string.Empty,
                ReviewedByMechanical = !lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ReviewedByMechanical : string.Empty,
                ReviewByProcess = !lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ReviewByProcess : string.Empty,
                ApprovedByLeadComplex = !lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ApprovedByLead : string.Empty,
                ApprovedByProjectComplex = !lineListRevision.IsSimpleRevisionBlock ? lineListRevision.ApprovedByProject : string.Empty,
                IsDraft = isDraft,
                IsIssued = hasBeenIssued,
                IsReserved = isReserved,
                IsLocked = lineListRevision.IsLocked,
                LockedOn = lineListRevision.LockedOn?.ToString() ?? string.Empty,
                LockedBy = lineListRevision.LockedBy,
                DateValidationMessage = lineListRevision.LockedOn.HasValue ? $"Please select a valid issue date. A valid date is either today's date, or a date in the past not before the Print For Issue Locked Date: {lineListRevision.LockedOn.Value.Date:yyyy-MM-dd}" : string.Empty,
                Facilities = facilities,
                CenovusProjects = cenovusProjects,
                ProjectTypes = projectTypes,
                Specifications = specifications,
                EPs = ePs,
                LineListStatuses = listListValidStatuses,
                Locations = locations,
                EPProjects = ePProjects,
                Areas = areas,
                IsEpUser = _currentUser.EpUser.Contains(lineListRevision.EpProjectId),
                IsEpAdmin = _currentUser.EpAdmin.Contains(lineListRevision.EpProjectId),
                IsCenovusAdmin = isCenovusAdmin,
                IsEpLeadEngineer = isEpLeadEngineer,
                IsEpDataEntry = isEpDataEntry,
                IsMostRecentRevision = isMostRecentRevision,
                IsEditable = isEditable,
                IsReadOnly = isReadOnly,
                CanRevertToDraft = isCenovusAdmin && !isDraft && isMostRecentRevision && !isReserved,
                CanPrintForIssue = !lineListRevision.IsLocked && !isReserved && isEditable && !hasBeenIssued,
                HasBeenIssued = hasBeenIssued,
                PrintLabel = printLabel,
                DisableDocumentNumber = !(isCenovusAdmin && isDraft && isMostRecentRevision),
                DisableEP = !_currentUser.IsCenovusAdmin,
                SaveVisible = isDraft && isEditable,

                PrintForIssueVisible = (_currentUser.IsCenovusAdmin || _currentUser.EppLeadEng.Contains(lineListRevision.EpProjectId) || _currentUser.EppDataEnt.Contains(lineListRevision.EpProjectId)),
                RevertToDraftVisible = isCenovusAdmin,
                DiscardDraftVisible = isEditable
            };

            model.PrintForIssueEnabled = isDraft && (_currentUser.IsCenovusAdmin || _currentUser.EppLeadEng.Contains(epProjectId) || _currentUser.EppDataEnt.Contains(epProjectId));
            model.LockedEnabled = !model.SaveVisible;

            model.DisablePreparedByProcess = model.DisablePreparedByMechanical = model.DisableReviewByProcess = model.DisableReviewedByMechanical = model.DisableApprovedByLeadComplex = model.DisableApprovedByProjectComplex = model.IsSimpleRevisionBlock;
            model.DisablePreparedBy = model.DisableReviewedBy = model.DisableApprovedByLead = model.DisableApprovedByProject = !model.IsSimpleRevisionBlock;

            if (lineListRevision.IsLocked || !isEditable || isReserved)
                LockFields(model, true, true, hasBeenIssued, isReserved && isEditable);
            else if (hasBeenIssued)
                LockFields(model, false, false, hasBeenIssued, false);


            if (_currentUser.IsCenovusAdmin)
            {
                model.CanRevertToDraft = !isDraft && isMostRecentRevision && !isReserved;
                model.DisableDocumentNumber = !(isDraft && isMostRecentRevision);
            }


            return PartialView("_Update", model);
        }

        private void LockFields(SearchLineListViewModel model, bool includeDescription, bool includeInitials, bool hasBeenIssued, bool canChangeLocationArea)
        {
            model.DisableActive = true;
            // chkActive.Enabled = false;

            if (includeDescription)
                model.DisableDescription = true;

            if (hasBeenIssued || includeInitials)
            {
                model.DisableComplexBlock = true;
                model.DisableSimpleBlock = true;
                model.DisableRevisionStatus = !includeInitials;
            }

            if (model.IsSimpleRevisionBlock || includeInitials)
            {

                model.DisablePreparedByProcess = true;
                model.DisablePreparedByMechanical = true;
                model.DisableReviewByProcess = true;
                model.DisableReviewedByMechanical = true;
                model.DisableApprovedByLeadComplex = true;
                model.DisableApprovedByProjectComplex = true;
            }

            if (!model.IsSimpleRevisionBlock || includeInitials)
            {
                model.DisablePreparedBy = true;
                model.DisableReviewedBy = true;
                model.DisableApprovedByLead = true;
                model.DisableApprovedByProject = true;
            }
            model.DisableDescription = true;


            model.DisableCenovusProject = true;
            model.DisableEP = true;
            model.DisableEPProject = true;
            model.DisableFacility = true;
            model.DisableLocation = !canChangeLocationArea;
            model.DisableArea = !canChangeLocationArea;
            model.DisableProjectType = true;
            model.DisableSpecification = true;
        }

        private List<Domain.Models.Area> GetAreas(Guid areaId, Guid facilityId, Guid specificationId, Guid locationId)
        {
            var data = _areaService.GetAll().Result.Where(a => a.IsActive);
            if (facilityId != Guid.Empty)
                data = data.Where(m => m.Location.FacilityId == facilityId);
            if (locationId != Guid.Empty)
                data = data.Where(m => m.LocationId == locationId);
            if (specificationId != Guid.Empty)
                data = data.Where(m => m.SpecificationId == specificationId);
            if (areaId != Guid.Empty)
                data.Union(new[] { _areaService.GetById(areaId).Result });

            return data.OrderBy(a => a.SortOrder).ToList();
        }

        private List<CenovusProject> GetCenovusProjects(Guid projectTypeId, Guid facilityId, Guid cenovusProjectId)
        {
            var data = _cenovusProjectService.GetAll().Result.Where(p => p.IsActive);
            if (projectTypeId != Guid.Empty)
                data = data.Where(m => m.ProjectTypeId == projectTypeId);
            if (facilityId != Guid.Empty)
                data = data.Where(m => m.FacilityId == facilityId);
            if (cenovusProjectId != Guid.Empty)
                data.Union(new[] { _cenovusProjectService.GetById(cenovusProjectId).Result });
            return data.ToList();
        }

        private List<Domain.Models.Location> GetLocations(Guid facilityId, Guid epProjectId, Guid locationId)
        {
            if (facilityId == Guid.Empty && epProjectId != Guid.Empty)
            {
                var project = _epProjectService.GetById(epProjectId).Result;
                facilityId = project.FacilityId.HasValue ? project.FacilityId.Value : Guid.Empty;
            }
            var data = _locationService.GetAll().Result.Where(p => p.IsActive);
            if (facilityId != Guid.Empty)
                data = data.Where(m => m.FacilityId == facilityId);

            if (locationId != Guid.Empty)
                data.Union(new[] { _locationService.GetById(locationId).Result });

            return data.ToList();
        }

        private List<Domain.Models.Specification> GetSpecifications(Guid facilityId, Guid specId)
        {

            var data = _specificationService.GetAll().Result.Where(p => p.IsActive);
            if (facilityId != Guid.Empty)
                data = data.Where(m => m.Areas.Any(n => n.Location.FacilityId == facilityId));

            if (specId != Guid.Empty)
                data.Union(new[] { _specificationService.GetById(specId).Result });

            return data.ToList();
        }

        private List<Domain.Models.LineListModel> GetDocumentNumber(Guid facilityId, Guid projectTypeId, Guid epCompanyId, Guid epProjectId, Guid cenovusProjectId)
        {

            var data = _lineListModelService.GetDocumentNumberAsync(facilityId, projectTypeId, epCompanyId, epProjectId, cenovusProjectId).Result;
            return data.OrderBy(m => m.DocumentNumber).ToList();
        }

        [HttpPost]
        public async Task<IActionResult> Update(SearchLineListViewModel model)
        {

            bool updateRevisions = false;

            // Retrieve the LineListRevision entity
            var lineListRevision = await _lineListRevisionService.GetById(model.LineListRevisionId);
            if (lineListRevision == null)
            {
                return Json(new { success = false, message = "Line list revision not found" });
            }

            lineListRevision.IsActive = model.IsActive;
            lineListRevision.IsSimpleRevisionBlock = model.IsSimpleRevisionBlock;

            lineListRevision.IsLocked = model.IsLocked;


            if (!lineListRevision.IsLocked)
            {
                lineListRevision.LocationId = model.SelectedLocationId;
                lineListRevision.EpProjectId = model.SelectedEPProjectId;
                lineListRevision.EpCompanyId = model.SelectedEPId;
                lineListRevision.AreaId = model.SelectedAreaId;
                lineListRevision.SpecificationId = model.SelectedSpecificationId;
                lineListRevision.SpecificationId = model.SelectedSpecificationId;


                if (model.IsSimpleRevisionBlock)
                {
                    lineListRevision.ApprovedByProject = model.ApprovedByProject;
                    lineListRevision.ApprovedByLead = model.ApprovedByLead;
                }
                else
                {
                    lineListRevision.ApprovedByProject = model.ApprovedByProjectComplex;
                    lineListRevision.ApprovedByLead = model.ApprovedByLeadComplex;
                }



                lineListRevision.PreparedByMechanical = model.PreparedByMechanical;
                lineListRevision.PreparedByProcess = model.PreparedByProcess;
                lineListRevision.ReviewedByMechanical = model.ReviewedByMechanical;
                lineListRevision.ReviewByProcess = model.ReviewByProcess;
                lineListRevision.Description = model.Description;
                lineListRevision.LineList.DocumentNumber = model.DocumentNumber;


                lineListRevision.PreparedBy = model.PreparedBy;
                lineListRevision.ReviewedBy = model.ReviewedBy;

            }

            if (lineListRevision.LineListStatusId != model.SelectedLineListStatusId)
            {
                updateRevisions = LineListRules.ChangeStatus(ref lineListRevision, model.SelectedLineListStatusId, _currentUser.Username, _lineRevisionService, _lineListStatusService, _lineRevisionOperatingModeService, _lineRevisionSegmentService);
            }

            lineListRevision.ModifiedBy = _currentUser.FullName;
            lineListRevision.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

            await _lineListRevisionService.Update(lineListRevision);

            if (updateRevisions)
                LineRules.UpdateRevisions(lineListRevision.Id, _lineRevisionService);

            // Return success response
            return Json(new { success = true });
        }
        [HttpGet]
        public async Task<IActionResult> GetUpRevData(Guid id)
        {
            var lineListRevision = await _lineListRevisionService.GetById(id);
            if (lineListRevision == null)
            {
                return Json(new { success = false, errorMessage = "Line list revision not found." });
            }

            var statusList = _lineListRules.GetValidStatusForUpRev(id).ToList();
            if (!statusList.Any())
            {
                return Json(new { success = false, errorMessage = "No valid statuses available for UpRev." });
            }

            var model = new SearchLineListViewModel
            {
                LineListRevisionId = lineListRevision.Id,
                DocumentNumber = lineListRevision.LineList?.DocumentNumber,
                DocumentRevision = lineListRevision.DocumentRevision,
                LineListStatusName = lineListRevision.LineListStatus?.Name,
                LineListStatuses = statusList,
                NextRevision = LineListRules.GetNextRevision(lineListRevision, statusList.First()),
                SelectedLineListStatusId = lineListRevision.LineListStatus?.DefaultUpRevStatusId != null
                                      && statusList.Any(s => s.Id == lineListRevision.LineListStatus.DefaultUpRevStatusId)
                                      ? lineListRevision.LineListStatus.DefaultUpRevStatusId.Value
                                      : statusList.First().Id
            };

            if (lineListRevision.LineListStatus?.DefaultUpRevStatusId != null &&
                statusList.Any(s => s.Id == lineListRevision.LineListStatus.DefaultUpRevStatusId))
            {
                model.SelectedLineListStatusId = lineListRevision.LineListStatus.DefaultUpRevStatusId.Value;
            }
            else
            {
                model.SelectedLineListStatusId = statusList.First().Id;
            }

            return PartialView("_UpRev", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetNextRevision(Guid lineListRevisionId, Guid newStatusId)
        {
            try
            {
                var lineListRevision = await _lineListRevisionService.GetById(lineListRevisionId);
                if (lineListRevision == null)
                {
                    return Json(new { success = false, errorMessage = "Line list revision not found." });
                }

                var newStatus = await _lineListStatusService.GetById(newStatusId);
                if (newStatus == null)
                {
                    return Json(new { success = false, errorMessage = "Selected status not found." });
                }

                var nextRevision = LineListRules.GetNextRevision(lineListRevision, newStatus);
                return Json(new { success = true, nextRevision });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Error calculating next revision: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpRevLineList(Guid lineListRevisionId, Guid newStatusId)
        {
            try
            {
                var lineListRevision = await _lineListRevisionService.GetById(lineListRevisionId);
                if (lineListRevision == null)
                {
                    return Json(new { success = false, errorMessage = "Line list revision not found." });
                }

                var newStatus = await _lineListStatusService.GetById(newStatusId);
                if (newStatus == null)
                {
                    return Json(new { success = false, errorMessage = "Selected status not found." });
                }

                var nextRevision = LineListRules.GetNextRevision(lineListRevision, newStatus);
                var username = User.Identity?.Name ?? "System";

                _lineListRules.UpRevLineList(lineListRevisionId, newStatusId, nextRevision, username);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Error performing UpRev: " + ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DiscardDraft(Guid id)
        {
            // Logic to delete the draft
            var result = await LineListRules.Discard(id);
            return Json(new { success = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetRevertToDraftData(Guid id)
        {
            try
            {
                var lineListRevision = await _lineListRevisionService.GetById(id);
                if (lineListRevision == null)
                {
                    return Json(new { success = false, errorMessage = "Line list revision not found." });
                }

                // If already a draft, we don't allow reverting
                if (lineListRevision.LineListStatus?.IsDraftOfId != null)
                {
                    return Json(new { success = false, errorMessage = "This revision is already a draft." });
                }

                var fromStatus = lineListRevision.LineListStatus?.Name_dash_Description ?? "Unknown";

                // Get the draft status using the IsIssuedOfId on the current status
                var issuedOfId = lineListRevision.LineListStatus?.IsIssuedOfId;
                if (issuedOfId == null || issuedOfId == Guid.Empty)
                {
                    return Json(new { success = false, errorMessage = "Draft status reference not found from current status." });
                }

                var allStatuses = await _lineListStatusService.GetAll();
                var draftStatus = allStatuses.FirstOrDefault(s => s.Id == issuedOfId);

                if (draftStatus == null)
                {
                    return Json(new { success = false, errorMessage = "Draft status not found." });
                }

                return Json(new
                {
                    success = true,
                    fromStatus,
                    toStatusId = draftStatus.Id,
                    toStatusName = draftStatus.Name_dash_Description
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Error loading revert data: " + ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> RevertToDraft(Guid lineListRevisionId, Guid newStatusId)
        {
            try
            {
                var lineListRevision = await _lineListRevisionService.GetById(lineListRevisionId);
                if (lineListRevision == null)
                {
                    return Json(new { success = false, errorMessage = "Line list revision not found." });
                }

                var newStatus = await _lineListStatusService.GetById(newStatusId);
                if (newStatus == null || newStatus.IsDraftOfId == null)
                {
                    return Json(new { success = false, errorMessage = "Invalid draft status selected." });
                }

                // Check if the user is a Cenovus admin
                bool isCenovusAdmin = _currentUser.IsCenovusAdmin; // Replace with actual logic
                if (!isCenovusAdmin)
                {
                    return Json(new { success = false, errorMessage = "Only Cenovus admins can revert to draft." });
                }

                // Updated to handle RuleResult return type from RevertToDraft
                var revertResult = await _lineListRules.RevertToDraft(lineListRevision, newStatusId);
                if (!revertResult.Success)
                {
                    return Json(new { success = false, errorMessage = revertResult.ErrorMessage });
                }

                //Added call to ActivePrevRevision to activate the previous revision after reverting
                LineListRules.ActivePrevRevision(lineListRevision, newStatusId);

                return Json(new { success = true, message = "Successfully reverted to draft." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = "Error reverting to draft: " + ex.Message });
            }
        }

        private async Task<byte[]> AppendSpecificationNotes(byte[] mainPdf, string[] specs)
        {
            using (MemoryStream outputStream = new MemoryStream())
            {
                using (var mainDoc = PdfReader.Open(new MemoryStream(mainPdf), PdfDocumentOpenMode.Import))
                using (var outputDoc = new PdfDocument())
                {
                    foreach (var page in mainDoc.Pages)
                    {
                        outputDoc.AddPage(page);
                    }

                    foreach (var spec in specs)
                    {
                        if (!string.IsNullOrEmpty(spec))
                        {
                            string specFilePath = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", $"LDT-{spec}-NOTES.pdf");
                            if (System.IO.File.Exists(specFilePath))
                            {
                                using (var specDoc = PdfReader.Open(specFilePath, PdfDocumentOpenMode.Import))
                                {
                                    foreach (var page in specDoc.Pages)
                                    {
                                        outputDoc.AddPage(page);
                                    }
                                }
                            }
                        }
                    }

                    outputDoc.Save(outputStream, false);
                }
                return outputStream.ToArray();
            }
        }

        [HttpPost]
        public async Task<IActionResult> PrintForReview(Guid id)
        {
            try
            {
                var rev = await _lineListRevisionService.GetById(id);
                if (rev == null)
                {
                    return Json(new { success = false, errorMessage = "Revision not found." });
                }

                return Json(new { success = true, id = id });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"Error printing for review: {ex.Message}" });
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> PrintForReviewResult(Guid id)
        //{
        //    try
        //    {
        //        Guid lineListRevisionId = id;

        //        // Step 1: Generate the main report into a MemoryStream
        //        var report = new LineDesignationReport(lineListRevisionId, showAllFields: true);
        //        var mainPdfStream = new MemoryStream();
        //        var specs = GenerateMainReportAsPdf(report, mainPdfStream); // ← You write this

        //        // Step 2: Merge all PDFs (main + spec notes)
        //        using var finalStream = new MemoryStream();
        //        var outputDocument = new PdfSharp.Pdf.PdfDocument();

        //        // Add main report pages
        //        var mainDoc = PdfReader.Open(mainPdfStream, PdfDocumentOpenMode.Import);
        //        foreach (var page in mainDoc.Pages)
        //            outputDocument.AddPage(page);

        //        // Add spec notes PDFs
        //        foreach (var spec in specs)
        //        {
        //            if (!string.IsNullOrWhiteSpace(spec))
        //            {
        //                var path = Path.Combine(_env.WebRootPath, "asset_src", $"LDT-{spec}-NOTES.pdf");
        //                if (System.IO.File.Exists(path))
        //                {
        //                    var notesDoc = PdfReader.Open(path, PdfDocumentOpenMode.Import);
        //                    foreach (var page in notesDoc.Pages)
        //                        outputDocument.AddPage(page);
        //                }
        //            }
        //        }

        //        outputDocument.Save(finalStream, false);
        //        finalStream.Position = 0;

        //        return File(finalStream.ToArray(), "application/pdf", $"LDT_{lineListRevisionId}_PrintForReview.pdf");
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error generating PDF: {ex.Message}");
        //    }
        //}

        private List<string> GenerateMainReportAsPdf(LineDesignationReport reportData, MemoryStream ms)
        {
            var doc = new Document();
            var section = doc.AddSection();
            section.PageSetup.Orientation = Orientation.Landscape;
            section.AddParagraph($"Revision Report for {reportData.LineListRevisionId}", "Heading1");

            // Add your data as tables/paragraphs, loop on Lines etc.

            var renderer = new PdfDocumentRenderer(true) { Document = doc };
            renderer.RenderDocument();
            renderer.PdfDocument.Save(ms, false);
            ms.Position = 0;

            return reportData.SpecsInReport?.ToList() ?? new List<string>();
        }

        [HttpGet]
        public async Task<IActionResult> PrintForReviewResult(Guid id, string mode = "")
        {
            try
            {
                var rev = await _lineListRevisionService.GetById(id);
                if (rev == null)
                    return NotFound("Revision not found.");

                var props = new
                {
                    EpCompanyId = rev.EpCompanyId,
                    EpProjectId = rev.EpProjectId,
                    IsHardRevision = rev.LineListStatus?.IsHardRevision ?? false,
                    IsIssued = rev.LineListStatus?.IsIssuedOfId != null
                };

                bool showAll = _currentUser.IsCenovusAdmin ||
                               _currentUser.EpUser.Contains(props.EpCompanyId) ||
                               _currentUser.EpAdmin.Contains(props.EpCompanyId) ||
                               (props.IsHardRevision && props.IsIssued);

                var reportData = new LineDesignationReport(id, showAll)
                {
                    Mode = mode
                };

                // Update revision description if printing for issue
                if (mode == "PrintForIssue")
                {
                    var currentRevision = reportData.Revisions.FirstOrDefault(r => r.LineListRevisionId == id);
                    if (currentRevision != null)
                        currentRevision.Description = rev.LineListStatus?.Name_dash_Description ?? "Issued";
                }

                return GeneratePdf(reportData);


            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating PDF: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult GeneratePdf(LineDesignationReport data, bool preview = false)
        {
          

            return new ViewAsPdf("_LineListCover", data)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.Tabloid,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                FileName = "LineListCover.pdf",
                ContentDisposition = preview ? Rotativa.AspNetCore.Options.ContentDisposition.Inline : Rotativa.AspNetCore.Options.ContentDisposition.Attachment,
                CustomSwitches = "--footer-center \"Page [page] of [toPage]\" --footer-font-size 9 --footer-spacing 5 --page-width 432mm --page-height 279mm"
            };
        }
        [HttpPost]
        public async Task<IActionResult> PrintForReference(Guid id)
        {
            try
            {
                var rev = await _lineListRevisionService.GetById(id);
                if (rev == null)
                    return Json(new { success = false, errorMessage = "Revision not found." });

                bool isDraft = rev.LineListStatus?.IsIssuedOfId == null && rev.IssuedOn == null;
                bool isIssued = rev.IssuedOn != null || rev.LineListStatus?.IsIssuedOfId != null;

                bool can = (!isDraft && isIssued)
                           || (isDraft && (User.IsInRole($"EpUser_{rev.EpCompanyId}") || User.IsInRole($"EpAdmin_{rev.EpCompanyId}") || User.IsInRole("CenovusAdmin")));

                if (!can)
                    return Json(new { success = false, errorMessage = "Cannot Print for Reference in this state." });

                return Json(new { success = true, id = id });

            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"Error printing for reference: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PrintForReferenceResult(Guid id)
        {
            try
            {
                var rev = await _lineListRevisionService.GetById(id);
                if (rev == null)
                    return NotFound("Revision not found.");

                // Build PDF via MigraDoc (similar to above, without watermark)
                var document = new Document();
                var section = document.AddSection();
                section.PageSetup.Orientation = Orientation.Landscape;
                section.PageSetup.PageFormat = PageFormat.P11x17;

                var title = section.AddParagraph("Line List Revision Report");
                title.Format.Font.Size = 16; title.Format.Font.Bold = true; title.Format.Alignment = ParagraphAlignment.Center;
                section.AddParagraph();

                var table = section.AddTable();
                table.Borders.Width = 0.5;
                table.AddColumn(Unit.FromCentimeter(5));
                table.AddColumn(Unit.FromCentimeter(10));

                var header = table.AddRow();
                header.HeadingFormat = true; header.Format.Font.Bold = true;
                header.Cells[0].AddParagraph("Field");
                header.Cells[1].AddParagraph("Value");

                void AddRowR(string field, string value)
                {
                    var r = table.AddRow();
                    r.Cells[0].AddParagraph(field);
                    r.Cells[1].AddParagraph(value);
                }

                AddRowR("Revision ID", rev.Id.ToString());
                AddRowR("Document Number", rev.LineList?.DocumentNumber ?? "N/A");
                AddRowR("Revision", rev.DocumentRevision);
                AddRowR("Status", rev.LineListStatus?.Name ?? "Unknown");
                AddRowR("Facility", rev.EpProject?.CenovusProject?.Facility?.Name ?? "N/A");
                AddRowR("Project", rev.EpProject?.CenovusProject?.Name ?? "N/A");
                AddRowR("Created On", rev.CreatedOn.ToString("yyyy-MM-dd"));
                AddRowR("Issued On", rev.IssuedOn?.ToString("yyyy-MM-dd") ?? "N/A");

                var renderer = new PdfDocumentRenderer(true) { Document = document };
                renderer.RenderDocument();

                using var ms = new MemoryStream();
                renderer.PdfDocument.Save(ms, false);
                return File(ms.ToArray(),
                            "application/pdf",
                            $"LDT_{rev.LineList?.DocumentNumber}_PrintForReference.pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating PDF: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PrintAndIssue(Guid id)
        {
            try
            {
                // 1) fetch
                var lineListRevision = await _lineListRevisionService.GetById(id);
                if (lineListRevision == null)
                    return Json(new { success = false, errorMessage = "Revision not found." });

                // 2) call PrintForIssue (locks if OK)
                var lockResult = await _lineListRules.PrintForIssue(lineListRevision, User.Identity.Name);
                if (!lockResult.Success)
                    return Json(new { success = false, errorMessage = $"PrintForIssue failed: {lockResult.ErrorMessage}" });

                // 3) now Issue
                var issueDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
                // Build the logo path (instead of Server.MapPath)
                string logoPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "assets",
                    "LDT-logo.png"
                );
                var issueResult = await _lineListRules.IssueLineList(
                    lineListRevision,
                    issueDate,
                    User.Identity.Name,
                     logoPath
                );
                if (!issueResult.Success)
                    return Json(new { success = false, errorMessage = $"IssueLineList failed: {issueResult.ErrorMessage}" });

                // 4) done
                return Json(new { success = true, id });


                //// only drafts may be printed & issued
                //bool isDraft = lineList.LineListStatus?.IsIssuedOfId == null && lineList.IssuedOn == null;
                //if (!isDraft)
                //{
                //    return Json(new { success = false, errorMessage = "Only a draft revision can be printed and issued." });
                //}

                //// must not already be locked
                //if (lineList.IsLocked)
                //{
                //    return Json(new { success = false, errorMessage = "This line list is already locked and cannot be issued again." });
                //}

                //bool isCenovusAdmin = User.IsInRole("CenovusAdmin");
                //bool isEpLeadEngineer = User.IsInRole($"EpLeadEngineer_{lineList.EpProjectId}");
                //bool isEpDataEntry = User.IsInRole($"EpDataEntry_{lineList.EpProjectId}");
                //if (!(isCenovusAdmin || isEpLeadEngineer || isEpDataEntry))
                //{
                //    return Json(new { success = false, errorMessage = "You do not have permission to print and issue this revision." });
                //}

                //var issuedStatus = (await _lineListStatusService.GetAll())
                //    .FirstOrDefault(s => s.IsIssuedOfId == lineList.LineListStatusId && s.IsActive);
                //if (issuedStatus == null)
                //{
                //    return Json(new { success = false, errorMessage = "Could not find the corresponding 'issued' status." });
                //}

                //lineList.LineListStatusId = issuedStatus.Id;
                //lineList.IssuedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
                //lineList.IsLocked = true; // Set Locked For Issue
                //lineList.ModifiedBy = _currentUser.FullName;
                //lineList.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

                //await _lineListRevisionService.Update(lineList);

                //return Json(new { success = true, id = id.ToString() });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = $"Error during print and issue: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> PrintAndIssueResult(Guid id)
        {
            try
            {
                var lineListRevision = await _lineListRevisionService.GetById(id);
                if (lineListRevision == null)
                    return NotFound("Revision not found.");

                // build the PDF (11×17, landscape, no watermark)
                var document = new Document();
                var section = document.AddSection();
                section.PageSetup.PageFormat = PageFormat.P11x17;
                section.PageSetup.Orientation = Orientation.Landscape;

                // cover page header
                var title = section.AddParagraph("Line Designation Table");
                title.Format.Font.Size = 18;
                title.Format.Font.Bold = true;
                title.Format.Alignment = ParagraphAlignment.Center;
                section.AddParagraph();

                // table builder helper
                Table table = null;
                void AddRow(string field, string value)
                {
                    if (table == null)
                    {
                        table = section.AddTable();
                        table.Borders.Width = 0.5;
                        table.AddColumn(Unit.FromCentimeter(6));
                        table.AddColumn(Unit.FromCentimeter(11));
                        var hdr = table.AddRow();
                        hdr.HeadingFormat = true;
                        hdr.Format.Font.Bold = true;
                        hdr.Cells[0].AddParagraph("Field");
                        hdr.Cells[1].AddParagraph("Value");
                    }

                    var row = table.AddRow();
                    row.Cells[0].AddParagraph(field);
                    row.Cells[1].AddParagraph(value);
                }

                AddRow("Document Number", lineListRevision.LineList?.DocumentNumber ?? "N/A");
                AddRow("Revision", lineListRevision.DocumentRevision);
                AddRow("Status", lineListRevision.LineListStatus?.Name);
                AddRow("Issue Date", lineListRevision.IssuedOn?.ToString("yyyy-MM-dd") ?? "");

                // any spec notes
                if (!string.IsNullOrWhiteSpace(lineListRevision.Specification?.Notes))
                {
                    section.AddParagraph();
                    section.AddParagraph("Specification Notes:");
                    section.AddParagraph(lineListRevision.Specification.Notes);
                }

                // render and return
                var renderer = new PdfDocumentRenderer(true) { Document = document };
                renderer.RenderDocument();
                using var ms = new MemoryStream();
                renderer.PdfDocument.Save(ms, false);

                var fileName = $"LDT_{lineListRevision.LineList?.DocumentNumber}_Issued.pdf";
                return File(ms.ToArray(), "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generating PDF: {ex.Message}");
            }
        }

    }
}
