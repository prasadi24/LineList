using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.ConcurrentEngineeringLine;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LineList.Cenovus.Com.UI.Controllers
{
    [Authorize(Policy = "Admin")]
    //[Authorize]
    public class ConcurrentEngineeringLineController : Controller
    {
        private readonly IConcurrentEngineeringLineService _concurrentEngineeringLineService;
        private readonly IMapper _mapper;
        private readonly IFacilityService _facilityService;
        private readonly IEpProjectService _epProjectService;
        private readonly ILineRevisionService _lineRevisionService;

        private readonly CurrentUser _currentUser;

        public ConcurrentEngineeringLineController(IConcurrentEngineeringLineService concurrentEngineeringLineService, IMapper mapper,
            IFacilityService facilityService, IEpProjectService epProjectService, CurrentUser currentUser,
            ILineRevisionService lineRevisionService)
        {
            _concurrentEngineeringLineService = concurrentEngineeringLineService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _facilityService = facilityService;
            _epProjectService = epProjectService;
            _currentUser = currentUser;
            _lineRevisionService = lineRevisionService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                ConcurrentEngineeringViewModel viewModel = new ConcurrentEngineeringViewModel();
                viewModel.Facilities = _facilityService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
                viewModel.EPProjects = _epProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
                viewModel.LDTFromDate = viewModel.LDTToDate = null;
                viewModel.IsCenovusAdmin = _currentUser.IsCenovusAdmin;
                return View(viewModel);
            }
            catch (Exception ex)
            {
                // Log error (consider using ILogger)
                return StatusCode(500, $"Error retrieving data: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<JsonResult> ConcurrentEngineeringLineFeed(Guid facilityId, Guid projectId, DateTime? ldtFromDate, DateTime? ldtToDate, bool showAsBuilt)
        {
            try
            {
                var lines = await _concurrentEngineeringLineService.GetFilteredLines(facilityId, projectId, ldtFromDate, ldtToDate, showAsBuilt);
                var lineDtos = _mapper.Map<IEnumerable<ConcurrentEngineeringLineResultDto>>(lines);
                return Json(new { data = lineDtos });
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error fetching data: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus([FromBody] List<KeyValuePair<Guid, bool>> activeSettings)
        {
            await _lineRevisionService.UpdateStatus(activeSettings);
            return Ok(new { message = "Update successful" });
        }
    }
}