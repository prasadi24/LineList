using AutoMapper;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.New.Controllers
{
    public class LineDBSearchController : Controller
    {
        private readonly ICommodityService _commodityService;
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
        private readonly ILineService _lineService;
        private readonly ILineRevisionService _lineRevisionService;
        private readonly IPipeSpecificationService _pipeSpecificationService;
        private readonly ILineStatusService _lineStatusService;
        private readonly IModularService _modularService;
        private readonly IMapper _mapper;
        private readonly CurrentUser _currentUser;

        public LineDBSearchController(
            ICommodityService commodityService,
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
            ILineService lineService,
            ILineRevisionService lineRevisionService,
            IPipeSpecificationService pipeSpecificationService,
            ILineStatusService lineStatusService,
            IModularService modularService,
            IMapper mapper,
            CurrentUser currentUser)
        {
            _commodityService = commodityService;
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
            _lineService = lineService;
            _pipeSpecificationService = pipeSpecificationService;
            _lineStatusService = lineStatusService;
            _lineRevisionService = lineRevisionService;
            _modularService = modularService;
            _mapper = mapper;
            _currentUser = currentUser;
        }

        public async Task<IActionResult> Index()
        {
            var commodities = _commodityService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var facilities = _facilityService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var cenovusProjects = _cenovusProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var projectTypes = _projectTypeService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var specifications = _specificationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var ePs = _epCompanyService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineListStatuses = _lineListStatusService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var locations = _locationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var ePProjects = _epProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var areas = _areaService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var pipeSpecifications = _pipeSpecificationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineStatuses = _lineStatusService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineLists = _lineListModelService.GetAll().Result.ToList();

            var model = new SearchLineListViewModel
            {
                Commodities = commodities,
                Facilities = facilities,
                CenovusProjects = cenovusProjects,
                ProjectTypes = projectTypes,
                Specifications = specifications,
                EPs = ePs,
                LineListStatuses = lineListStatuses,
                Locations = locations,
                EPProjects = ePProjects,
                Areas = areas,
                PipeSpecifications = pipeSpecifications,
                LineStatuses = lineStatuses,
                LineLists = lineLists//,
                                     //ModularDetails = modulardetails
            };


            model.ShowDrafts = false;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> SearchResult(SearchLineListViewModel model)
        {
            // fetch the flattened DTOs directly
            var results = await _lineRevisionService.GetFilteredLineRevisions(
                model.SelectedFacilityId != Guid.Empty ? model.SelectedFacilityId : (Guid?)null,
                model.SelectedSpecificationId != Guid.Empty ? model.SelectedSpecificationId : (Guid?)null,
                model.SelectedLocationId != Guid.Empty ? model.SelectedLocationId : (Guid?)null,
                model.SelectedCommodityId != Guid.Empty ? model.SelectedCommodityId : (Guid?)null,
                model.SelectedAreaId != Guid.Empty ? model.SelectedAreaId : (Guid?)null,
                model.SelectedcenovusProjectId != Guid.Empty ? model.SelectedcenovusProjectId : (Guid?)null,
                model.SelectedEPProjectId != Guid.Empty ? model.SelectedEPProjectId : (Guid?)null,
                model.SelectedPipeSpecificationId != Guid.Empty ? model.SelectedPipeSpecificationId : (Guid?)null,
                model.SelectedLineStatusId != Guid.Empty ? model.SelectedLineStatusId : (Guid?)null,
                model.ShowDrafts,
                model.ShowOnlyActive,
                model.SelectedDocumentNumberId,
                model.SelectedModularID,
                string.IsNullOrWhiteSpace(model.SelectedSequenceNumber)
                    ? null
                    : model.SelectedSequenceNumber,
                model.SelectedProjectTypeId != Guid.Empty ? model.SelectedProjectTypeId : (Guid?)null
            );

            // security‐trimming pass
            foreach (var dto in results)
            {
                bool isHardIssued = dto.IsHardRevision && dto.IsIssued;
                bool canViewFull = _currentUser.IsCenovusAdmin;

                // EP‐roles see everything
                if (!canViewFull)
                {
                    canViewFull = _currentUser.IsEpAdmin
                               || _currentUser.IsEpUser
                               || _currentUser.EppLeadEng.Any(id => id == dto.EpProjectId)
                               || _currentUser.EppDataEnt.Any(id => id == dto.EpProjectId)
                               || _currentUser.EppRsv.Any(id => id == dto.EpProjectId);
                }

                // Read-only sees hard+issued or their EP‐project
                if (!canViewFull && _currentUser.IsReadOnly)
                {
                    canViewFull = isHardIssued
                               || _currentUser.EpUser.Any(id => id == dto.EpProjectId);
                }

                // Everyone else only sees hard+issued
                if (!canViewFull && !_currentUser.IsReadOnly)
                {
                    canViewFull = isHardIssued;
                }

                if (!canViewFull)
                {
                    // blank‐out all but the “allowed subset”
                    dto.AreaName = null;
                    dto.CenovusProjectName = null;
                    dto.EpCompanyName = null;
                    dto.EpProjectName = null;
                    dto.FacilityName = null;
                    dto.ProjectTypeName = null;
                    dto.PipeSpecificationName = null;
                    dto.SizeNpsName = null;
                    dto.ParentChild = null;
                    dto.CreatedBy = null;
                    dto.CreatedOn = default;
                    dto.ModifiedBy = null;
                    dto.ModifiedOn = null;
                }
            }

            return Json(results);
        }
    }
}