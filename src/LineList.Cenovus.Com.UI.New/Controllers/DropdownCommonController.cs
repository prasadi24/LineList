using AutoMapper;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.New.Controllers
{
    public class DropdownCommonController : Controller
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
        private readonly ILineListRevisionService _lineListRevisionService;
        private readonly IPipeSpecificationService _pipeSpecificationService;
        private readonly IModularService _modularService;
        private readonly IMapper _mapper;

        public DropdownCommonController(
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
           ILineListRevisionService lineListRevisionService,
           IPipeSpecificationService pipeSpecificationService,
           IModularService modularService,
           IMapper mapper)
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
            _lineListRevisionService = lineListRevisionService;
            _pipeSpecificationService = pipeSpecificationService;
            _modularService = modularService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDependentDropdowns(Guid facilityId, Guid cenovusProjectId, Guid projectTypeId, Guid specificationsId, Guid epCompanyId, Guid lineListStatusId, Guid locationId, Guid epProjectId, Guid documentNumberId)
        {

            // Fetch modular details
            var mins = await _modularService.GetModularDetails(
                facilityId,
                projectTypeId,
                epCompanyId,
                epProjectId,
                cenovusProjectId
            );

            // Fetch & filter specifications
            var specifications = await _specificationService.GetAll();
            if (facilityId != Guid.Empty)
                specifications = specifications.Where(s => s.Areas.Any(a => a.Location.FacilityId == facilityId));
            specifications = specifications.Where(s => s.IsActive);

            // Fetch & filter EP Projects
            var epProjects = await _epProjectService.GetAll();
            //if (cenovusProjectId != Guid.Empty)
            //    epProjects = epProjects.Where(n => n.CenovusProjectId == cenovusProjectId);
            if (epCompanyId != Guid.Empty)
                epProjects = epProjects.Where(n => n.EpCompanyId == epCompanyId);
            if (facilityId != Guid.Empty)
                epProjects = epProjects.Where(n => n.FacilityId == facilityId);
            if (projectTypeId != Guid.Empty)
                epProjects = epProjects.Where(n => n.ProjectTypeId == projectTypeId);
            epProjects = epProjects.Where(p => p.IsActive);

            // Fetch & filter locations
            var locations = await _locationService.GetAll();
            if (projectTypeId != Guid.Empty)
                locations = locations.Where(p => p.FacilityId == facilityId);
            locations = locations.Where(p => p.IsActive);

            // Fetch & filter areas
            var areas = await _areaService.GetAll();
            if (facilityId != Guid.Empty)
                areas = areas.Where(p => p.Location.FacilityId == facilityId);
            if (locationId != Guid.Empty)
                areas = areas.Where(p => p.LocationId == locationId);
            if (specificationsId != Guid.Empty)
                areas = areas.Where(p => p.SpecificationId == specificationsId);

            // Fetch & filter Cenovus Projects
            var cenovusProjects = await _cenovusProjectService.GetAll();
            if (facilityId != Guid.Empty)
                cenovusProjects = cenovusProjects.Where(c => c.FacilityId == facilityId);
            if (projectTypeId != Guid.Empty)
                cenovusProjects = cenovusProjects.Where(c => c.ProjectTypeId == projectTypeId);

            cenovusProjects = cenovusProjects.Where(c => c.IsActive);
            // Fetch & filter commodities
            var commodities = await _commodityService.GetAll();

            // Filter commodities by SpecificationId
            if (specificationsId != Guid.Empty)
                commodities = commodities.Where(c => c.SpecificationId == specificationsId).ToList();

           

            // Only active commodities
            commodities = commodities.Where(c => c.IsActive).ToList();

            // Fetch & filter PipeSpecifications
            var pipeSpecifications = await _pipeSpecificationService.GetAll();
            if (specificationsId != Guid.Empty)
                pipeSpecifications = pipeSpecifications.Where(p => p.SpecificationId == specificationsId);
            //if (facilityId != Guid.Empty)
            //    pipeSpecifications = pipeSpecifications.Where(p => p.Specification.Areas.Any(a => a.Location.FacilityId == facilityId));
            pipeSpecifications = pipeSpecifications.Where(p => p.IsActive).ToList();

            var data = await _lineListModelService.GetDocumentNumberAsync(facilityId,projectTypeId,epCompanyId,epProjectId,cenovusProjectId);
            var documentNumbers = data.Select(d => new { id = d.Id, name = d.DocumentNumber }).ToList();

            return Json(new
            {
                documentNumbers,
                mins,
                specifications,
                epProjects,
                locations,
                areas,
                cenovusProjects,
                commodities,
                pipeSpecifications
            });
        }
    }
}