using AutoMapper;
using LineList.Cenovus.Com.API.DataTransferObjects.ConcurrentEngineeringLine;
using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using LineList.Cenovus.Com.RulesEngine;
using LineList.Cenovus.Com.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace LineList.Cenovus.Com.UI.Controllers
{
    public class LineCollectionController : Controller
    {
        private readonly ISpecificationService _specificationService;
        private readonly ILocationService _locationService;
        private readonly IAreaService _areaService;
        private readonly IPipeSpecificationService _pipeSpecificationService;
        private readonly ICommodityService _commodityService;
        private readonly ILineStatusService _lineStatusService;
        private readonly ILineService _lineService;
        private readonly ILineListRevisionService _lineListRevisionService;
        private readonly ILineRevisionService _lineRevisionService;
        private readonly ISizeNpsService _sizeNpsService;
        private readonly ILineRevisionOperatingModeService _lineRevisionOperatingModeService;
        private readonly IFluidPhaseService _fluidPhaseService;
        private readonly IPressureProtectionService _pressureProtectionService;
        private readonly IScheduleService _scheduleService;
        private readonly ICorrosionAllowanceService _corrosionAllowanceService;
        private readonly IXrayService _xrayService;
        private readonly INdeCategoryService _ndeCategoryService;
        private readonly IStressAnalysisService _stressAnalysisService;
        private readonly ICodeService _codeService;
        private readonly ITestMediumService _testMediumService;
        private readonly IPostWeldHeatTreatmentService _pwhtService;
        private readonly IFluidService _fluidService;
        private readonly ICsaClassLocationService _csaClassLocationService;
        private readonly ICsaHvpLvpService _csaHvpLvpService;

        private readonly IFacilityService _facilityService;
        private readonly ICenovusProjectService _cenovusProjectService;
        private readonly IEpProjectService _epProjectService;
        private readonly IProjectTypeService _projectTypeService;
        private readonly ILineListStatusService _lineListStatusService;
        private readonly ILineListModelService _lineListModelService;
        private readonly IEpCompanyService _epCompanyService;
        private readonly ISegmentTypeService _segmentTypeService;
        private readonly IOperatingModeService _operatingModeService;
        private readonly ILineRevisionSegmentService _lineRevisionSegmentService;

        private readonly IUserPreferenceService _userPreferenceService;
        private readonly CurrentUser _currentUser;
        private readonly IInsulationThicknessService _insulationThicknessService;
        private readonly ITracingTypeService _tracingTypeService;
        private readonly IInsulationTypeService _insulationTypeService;
        private readonly IMapper _mapper;

        private readonly LineListDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<LineCollectionController> _logger;


        public LineCollectionController(
            ISpecificationService specificationService,
            ILocationService locationService,
            IAreaService areaService,
            IPipeSpecificationService pipeSpecificationService,
            ICommodityService commodityService,
            ILineStatusService lineStatusService,
            ILineService lineService,
            ILineListRevisionService lineListRevisionService,
            ILineRevisionService lineRevisionService,
            ISizeNpsService sizeNpsService,
            ILineRevisionOperatingModeService lineRevisionOperatingModeService,
            IFluidPhaseService fluidPhaseService,
            IPressureProtectionService pressureProtectionService,
            IScheduleService scheduleService,
            ICorrosionAllowanceService corrosionAllowanceService,
            IXrayService xrayService,
            INdeCategoryService ndeCategoryService,
            IStressAnalysisService stressAnalysisService,
            ICodeService codeService,
            ITestMediumService testMediumService,
            IPostWeldHeatTreatmentService pwhtService,
            IFluidService fluidService,
            ICsaClassLocationService csaClassLocationService,
            ICsaHvpLvpService csaHvpLvpService,

            IFacilityService facilityService,
            ICenovusProjectService cenovusProjectService,
            IEpCompanyService epCompanyService,
            IEpProjectService epProjectService,
            IProjectTypeService projectTypeService,
            ILineListStatusService lineListStatusService,
            ILineListModelService lineListModelService,
            ISegmentTypeService segmentTypeService,
            IOperatingModeService operatingModeService,
            ILineRevisionSegmentService lineRevisionSegmentService,

            IUserPreferenceService userPreferenceService,
            CurrentUser currentUser,
            IInsulationThicknessService insulationThicknessService,
            ITracingTypeService tracingTypeService,
            IInsulationTypeService insulationTypeService,
            IMapper mapper,
            LineListDbContext dbContext,
            IMemoryCache memoryCache,
            ILogger<LineCollectionController> logger
            )
        {
            _specificationService = specificationService;
            _locationService = locationService;
            _areaService = areaService;
            _pipeSpecificationService = pipeSpecificationService;
            _commodityService = commodityService;
            _lineStatusService = lineStatusService;
            _lineService = lineService;
            _lineListRevisionService = lineListRevisionService;
            _lineRevisionService = lineRevisionService;
            _sizeNpsService = sizeNpsService;
            _lineRevisionOperatingModeService = lineRevisionOperatingModeService;
            _fluidPhaseService = fluidPhaseService;
            _pressureProtectionService = pressureProtectionService;
            _scheduleService = scheduleService;
            _corrosionAllowanceService = corrosionAllowanceService;
            _xrayService = xrayService;
            _ndeCategoryService = ndeCategoryService;
            _stressAnalysisService = stressAnalysisService;
            _codeService = codeService;
            _testMediumService = testMediumService;
            _pwhtService = pwhtService;
            _fluidService = fluidService;
            _csaClassLocationService = csaClassLocationService;
            _csaHvpLvpService = csaHvpLvpService;

            _facilityService = facilityService;
            _cenovusProjectService = cenovusProjectService;
            _epCompanyService = epCompanyService;
            _epProjectService = epProjectService;
            _projectTypeService = projectTypeService;
            _lineListStatusService = lineListStatusService;
            _lineListModelService = lineListModelService;
            _segmentTypeService = segmentTypeService;
            _operatingModeService = operatingModeService;
            _lineRevisionSegmentService = lineRevisionSegmentService;

            _userPreferenceService = userPreferenceService;
            _currentUser = currentUser;
            _insulationThicknessService = insulationThicknessService;
            _tracingTypeService = tracingTypeService;
            _insulationTypeService = insulationTypeService;
            _mapper = mapper;
            _dbContext = dbContext;
            _memoryCache = memoryCache;
            _logger = logger;


        }

        [HttpGet]
        public async Task<IActionResult> Index(Guid lineListRevisionId)
        {
            ViewBag.LineListRevisionId = lineListRevisionId;
            var lines = (await _lineRevisionService.GetByLineListRevisionId(lineListRevisionId));
            var userNames = lines.Select(p => p.CheckedOutBy).Where(u => !string.IsNullOrEmpty(u)).Distinct().ToList();

            var allUserPrefs = await _userPreferenceService.GetAll();

            var userPrefDict = allUserPrefs.Where(up => userNames.Contains(up.UserName))
                                            .ToDictionary(up => up.UserName, up => up.FullName);

            var allLines = _lineService.GetAll().Result.ToList();
            var parentLineLookup = new HashSet<(Guid LocationId, Guid CommodityId, string SequenceNumber)>(
                                            allLines.Where(c => c.ChildNumber != 0) // Filter parent lines
                                                      .Select(c => (c.LocationId, c.CommodityId, c.SequenceNumber))
                                        );
            var ll = await _lineListRevisionService.GetById(lineListRevisionId);
            var reservedLineListRevisionId = await _lineListRevisionService.GetReservedLineListRevisionIdByProjectId(ll.EpProjectId);

            var results = new List<LineCollectionViewModel>();
            foreach (var p in lines)
            {
                var resultDto = new LineCollectionViewModel();

                resultDto.Id = p.Id;
                resultDto.LineId = p?.Line?.Id ?? Guid.Empty;
                resultDto.AreaName = p?.Area?.Name ?? string.Empty;
                resultDto.CheckedOutBy = (!string.IsNullOrEmpty(p.CheckedOutBy) && userPrefDict.ContainsKey(p.CheckedOutBy))
                                            ? userPrefDict[p.CheckedOutBy]
                                            : p.CheckedOutBy;
                //userPrefDict.TryGetValue(p.CheckedOutBy, out var fullName);
                //resultDto.CheckedOutBy = string.IsNullOrEmpty(fullName) ? p.CheckedOutBy : fullName;
                resultDto.CommodityName = p?.Line?.Commodity?.Name ?? string.Empty;
                resultDto.DocumentNumber = p?.LineListRevision?.LineListDocumentNumber;
                resultDto.SizeNpsPipeName = p?.SizeNpsPipe?.Name ?? string.Empty;
                resultDto.LineStatusName = p?.LineStatus?.Name ?? string.Empty;
                resultDto.LocationName = p?.Line?.Location.Name ?? string.Empty;
                resultDto.ParentChild = (p.Line.ChildNumber != 0) ? "C" :
                   (parentLineLookup.Contains((p.Line.LocationId, p.Line.CommodityId, p.Line.SequenceNumber))) ? "p" : string.Empty;
                resultDto.Revision = p.Revision;
                resultDto.SequenceNumber = p?.Line?.SequenceNumber ?? string.Empty;
                resultDto.SpecificationName = p?.Specification?.Name ?? string.Empty;
                resultDto.ValidationState = p.ValidationState;
                resultDto.PipeSpecificationName = p?.PipeSpecification?.Name ?? string.Empty;
                resultDto.LineListRevisionId = p.LineListRevisionId;
                resultDto.ChildNumber = p.Line.ChildNumber;
                resultDto.ModularId = p?.Line?.ModularId ?? string.Empty;
                resultDto.ReservedLineListRevisionId = reservedLineListRevisionId;

                results.Add(resultDto);
            }

            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> CheckOutLines([FromBody] LineCheckIn_CheckOutRequest request)
        {
            var lineIds = request.LineIds;
            if (lineIds == null || !lineIds.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No line IDs were provided."
                });
            }

            var currentUser = _currentUser.FullName;

            if (string.IsNullOrWhiteSpace(currentUser))
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Unauthorized: Unable to determine current user."
                });
            }

            // Call shared business logic
            int numRowsCheckedOut = await LineRules.CheckOutLines(lineIds.ToArray(), currentUser, _lineRevisionService);
            bool partialSuccess = numRowsCheckedOut > 0 && numRowsCheckedOut < lineIds.Count;

            if (numRowsCheckedOut == 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "No lines you have selected were able to be checked out."
                });
            }

            return Ok(new
            {
                success = true,
                message = partialSuccess
                    ? "Some lines were checked out successfully. Others were already checked out by another user."
                    : "All selected lines are now checked out to you.",
                linesCheckedOut = numRowsCheckedOut
            });
        }

        [HttpPost]
        public async Task<IActionResult> CheckInLines([FromBody] LineCheckIn_CheckOutRequest request)
        {
            var lineIds = request.LineIds;
            var lineListRevisionId = request.LineListRevisionId;
            var currentLineListRevision =  _lineListRevisionService.GetById(lineListRevisionId).Result;
            if (lineIds == null || !lineIds.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No line IDs were provided."
                });
            }

            CurrentUser user = _currentUser;
            var epCompanyId = currentLineListRevision.EpCompanyId;

            if (string.IsNullOrWhiteSpace(user.Username))
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Unauthorized: Unable to determine current user."
                });
            }

            int numRowsCheckedIn = await LineRules.CheckInLines(lineIds.ToArray(), epCompanyId, user, _lineRevisionService);

            if (numRowsCheckedIn == 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "No lines were checked in. Ensure they are checked out to you."
                });
            }

            return Ok(new
            {
                success = true,
                message = $"Selected line (that were checked out to you only) are now checked in."
            });
        }

        [HttpPost]
        public IActionResult CopyLineId([FromBody] Guid lineId)
        {
            if (lineId == Guid.Empty)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid line ID provided."
                });
            }

            HttpContext.Session.SetString("IdToCopy", lineId.ToString());

            return Ok(new
            {
                success = true,
                message = "Attributes for this row have been copied. Select one or more rows and choose 'Paste'."
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateChildLine(Guid lineRevisionId)
        {           
            string currentUser = _currentUser.FullName;
            var selectedLineRev = await _lineRevisionService.GetById(lineRevisionId);

            // Check if the line revision exists
            if (selectedLineRev == null)
            {
                return BadRequest(new { success = false, message = "Line Revision not found." });
            }

            // Check if the line revision is checked out by the current user
            if (selectedLineRev.CheckedOutBy != currentUser)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "You must check out a line if you wish to add a child line to it."
                });
            }

            // Proceed to create the child line
            try
            {
                var childLine = await LineRules.CreateChild(
                    lineRevisionId,
                    currentUser,
                    _lineRevisionService,
                    _lineListRevisionService,
                    _lineService,
                    _lineRevisionSegmentService,
                    _lineRevisionOperatingModeService
                );

                // Return success response with the new child line information
                return Json(new
                {
                    success = true,
                    message = $"Child line has been successfully added for line with sequence number {selectedLineRev.Line.SequenceNumber}"
                });
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors
                return StatusCode(500, new
                {
                    success = false,
                    message = $"An error occurred while creating the child line: {ex.Message}"
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PasteLineAttributes([FromBody] PasteAttributesRequest request)
        {
            if (request == null || request.ToLineIds == null || !request.ToLineIds.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No lines have been selected to paste attribute data to."
                });
            }

            // Read the copied line ID from session
            var fromLineIdStr = HttpContext.Session.GetString("IdToCopy");
            if (string.IsNullOrEmpty(fromLineIdStr) || !Guid.TryParse(fromLineIdStr, out var fromLineId))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "No line attributes copied. Select the 'copy' button for a single row."
                });
            }

            var user = _currentUser;

            int countSuccessful = await LineRules.PasteLineAttributes(
                fromLineId,
                request.ToLineIds,
                user.FullName,
                _lineRevisionService,
                _lineRevisionSegmentService,
                _lineRevisionOperatingModeService,
                _locationService,
                _commodityService,
                _pipeSpecificationService,
                _sizeNpsService,
                _insulationThicknessService,
                _tracingTypeService,
                _insulationTypeService
            );

            int countAttempted = request.ToLineIds.Length;

            if (countAttempted > 0 && countAttempted == countSuccessful)
            {
                return Ok(new
                {
                    success = true,
                    message = "Attributes from copied line have been pasted into selected lines."
                });
            }
            else if (countSuccessful > 0)
            {
                return Ok(new
                {
                    success = false,
                    message = "Attributes from copied line have been pasted into selected lines that are checked out to you. Only lines that share the same Specification as the copied line were pasted."
                });
            }
            else
            {
                return Ok(new
                {
                    success = false,
                    message = "No selected lines were checked out to you, or the copied & pasted lines don't share the same Specification."
                });
            }
        }

        [HttpGet]
        public async Task<IActionResult> MoveSelectedLines(Guid lineListRevisionId)
        {
            var ePProjects = _epProjectService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineLists = _lineListModelService.GetAll().Result.ToList();
            var viewModel = new MoveLinesViewModel
            {
                LineListRevisionId = lineListRevisionId,
                EPProjects = ePProjects,
                LineLists = lineLists,
                SelectedEPProjectId = Guid.Empty,
                SelectedLineListId = Guid.Empty
            };

            return PartialView("_MoveLines", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ImportLines(IFormFile file, Guid lineListRevisionId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            await _lineService.ImportLinesFromExcel(stream, lineListRevisionId); // service layer
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DiscardLines([FromBody] List<Guid> lineRevisionIds)
        {
            if (lineRevisionIds == null || !lineRevisionIds.Any())
                return BadRequest("No lines selected for discard.");

            var currentUser = _currentUser.FullName;

            // Filter lines that can be discarded
            var validLineIds = await LineRules.CanBeDiscarded(lineRevisionIds.ToArray(), currentUser, _lineRevisionService);

            if (!validLineIds.Any())
            {
                return BadRequest("No lines selected, Select one or more lines that are checked out to you to discard. Lines must either be 'RESERVED', or have never issued on this line list. Parents with children cannot be discarded, until all their children are discarded.");
            }

            var discarded = new List<Guid>();

            foreach (var id in validLineIds)
            {
                var line = await _lineRevisionService.GetById(id);
                if (line != null)
                {
                    await LineRules.DeleteLine(line, currentUser, _lineRevisionService, _lineRevisionSegmentService, _lineRevisionOperatingModeService, _lineStatusService);
                    discarded.Add(id);
                }
            }

            return Ok(new { success = true, discarded });
        }


        [HttpGet]
        public async Task<IActionResult> Update(Guid lineId, string lineRev)
        {
            var lineDetails = _lineRevisionService.GetByLineId(lineId).Result
                .Where(line => line.Revision == lineRev && line.IsActive).FirstOrDefault();


            var areas = _areaService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var pipeSpecifications = _pipeSpecificationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var sizeNpsPipes = _sizeNpsService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var lineStatuses = _lineStatusService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var fluidPhases = _fluidPhaseService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var pressureProtections = _pressureProtectionService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var schedules = _scheduleService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var corrosionAllowances = _corrosionAllowanceService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var xrays = _xrayService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var ndeCategories = _ndeCategoryService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var stressAnalyses = _stressAnalysisService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var codes = _codeService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var testMediums = _testMediumService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var pwhtOptions = _pwhtService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var fluids = _fluidService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var csaClassLocations = _csaClassLocationService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();
            var csaHvpLvps = _csaHvpLvpService.GetAll().Result.Where(o => o.IsActive).OrderBy(o => o.SortOrder).ToList();

            if (lineDetails == null)
                return NotFound();

            var operatingModes = (await _operatingModeService.GetAll())
                                   .Where(o => o.IsActive)
                                   .OrderBy(o => o.SortOrder)
                                   .ToList();

            LineRevisionOperatingMode primaryOpMode = null;
            if (lineDetails.LineRevisionOperatingModes.Any(x => x.OperatingModeNumber == "1"))
            {
                primaryOpMode = lineDetails.LineRevisionOperatingModes.Single(x => x.OperatingModeNumber == "1");
            }
            var allModes = lineDetails.LineRevisionOperatingModes.ToList();
            // drop the primary 1st mode:

            //string operatingModeIdDescription;
            //if (primaryOpMode?.OperatingModeId != null)
            //{
            //    var om = operatingModes
            //        .FirstOrDefault(m => m.Id == primaryOpMode.OperatingModeId.Value);
            //    operatingModeIdDescription = om != null
            //        ? om.Description
            //        : "N/A";
            //}
            //else
            //{
            //    operatingModeIdDescription = "N/A";
            //}



            List<LineRevisionSegment> Segments = new List<LineRevisionSegment>();
            if (lineDetails.LineRevisionSegments != null)
            {
                Segments = lineDetails.LineRevisionSegments.ToList();

            }
            //.Where(m => m.TracingType != null && m.TracingType.IsJacketed);

            //bool jacketed = false;
            //if (Segments.Any())
            //{
            //    Segment = Segments.First();                
            //    jacketed = true;
            //}

            LineCollectionViewModel Model = new LineCollectionViewModel()
            {
                Areas = areas,
                PipeSpecifications = pipeSpecifications,
                SizeNpsPipes = sizeNpsPipes,
                LineStatuses = lineStatuses,
                FluidPhases = fluidPhases,
                PressureProtections = pressureProtections,
                Schedules = schedules,
                CorrosionAllowances = corrosionAllowances,
                Xrays = xrays,
                NDECategories = ndeCategories,
                StressAnalyses = stressAnalyses,
                Codes = codes,
                TestMediums = testMediums,
                PWHTOptions = pwhtOptions,
                Fluids = fluids,
                CsaClassLocations = csaClassLocations,
                CsaHvpLvps = csaHvpLvps,
                OperatingModes = operatingModes,

                // Line Identification
                LocationName = lineDetails.Line.Location != null ? lineDetails.Line.Location.Name : string.Empty,
                CommodityName = lineDetails.Line.Commodity != null ? lineDetails.Line.Commodity.Name : string.Empty,
                SpecificationName = lineDetails.Specification != null ? lineDetails.Specification.Name : string.Empty,
                AreaId = lineDetails.AreaId.HasValue ? lineDetails.AreaId.Value : Guid.Empty,
                PipeSpecificationId = lineDetails.PipeSpecificationId.HasValue ? lineDetails.PipeSpecificationId.Value : Guid.Empty,
                SizeNpsPipeId = lineDetails.SizeNpsPipeId.HasValue ? lineDetails.SizeNpsPipeId.Value : Guid.Empty,
                SequenceNumber = lineDetails.Line.SequenceNumber.ToString(),
                Revision = lineDetails.Revision,
                OriginatingPID = lineDetails.OriginatingPID,
                IsChild = lineDetails.Line.ChildNumber > 0,
                ModularId = lineDetails.Line.ModularId != null ? lineDetails.Line.ModularId : string.Empty,
                LineRoutingFrom = primaryOpMode != null ? primaryOpMode.LineRoutingFrom : string.Empty,
                OperatingModeNumber = primaryOpMode != null ? primaryOpMode.OperatingModeNumber : string.Empty,
                OperatingModeId = primaryOpMode.OperatingModeId.HasValue ? primaryOpMode.OperatingModeId.Value : Guid.Empty,
                OperatingModeIdDescription = primaryOpMode?.OperatingModeId.HasValue == true
                ? operatingModes.FirstOrDefault(m => m.Id == primaryOpMode.OperatingModeId)?.Description
                : "N/A",

                //OperatingModeIdDescription = ,
                LineRoutingTo = primaryOpMode != null ? primaryOpMode.LineRoutingTo : string.Empty,
                LineRevisionStatus = lineDetails.LineStatus.Name != null ? lineDetails.LineStatus.Name : string.Empty,

                // Process Attributes
                OperatingPressurePipe = primaryOpMode != null ? primaryOpMode.OperatingPressurePipe : string.Empty,
                OperatingTemp = primaryOpMode != null ? primaryOpMode.OperatingTemperaturePipe : string.Empty,
                FluidPhaseId = primaryOpMode.FluidPhaseId.HasValue ? primaryOpMode.FluidPhaseId.Value : Guid.Empty,
                FluidPhaseName = primaryOpMode != null ? primaryOpMode.FluidPhase != null ? primaryOpMode.FluidPhase.Name : string.Empty : string.Empty,
                DesignPressure = lineDetails.DesignPressurePipe,
                DesignTempMax = lineDetails.DesignTemperatureMaximumPipe,
                DesignTempMin = lineDetails.DesignTemperatureMinimumPipe,
                Notes = primaryOpMode != null ? primaryOpMode.Notes : string.Empty,
                ExpansionTemp = lineDetails.ExpansionTemperature,
                UpsetPressure = lineDetails.UpsetPressurePipe,
                UpsetTemp = lineDetails.UpsetTemperaturePipe,
                //PressureProtectionId = primaryOpMode.PressureProtectionId.HasValue ? primaryOpMode.PressureProtectionId.Value : Guid.Empty,
                PressureProtectionName = primaryOpMode != null ? primaryOpMode.PressureProtection != null ? primaryOpMode.PressureProtection.Name : string.Empty : string.Empty,

                // Mechanical Attributes
                ScheduleId = lineDetails.SchedulePipeId.HasValue ? lineDetails.SchedulePipeId.Value : Guid.Empty,
                WallThickness = lineDetails.WallThicknessPipe.ToString(),
                TestPressure = lineDetails.TestPressurePipe,
                TestMediumId = lineDetails.TestMediumPipeId.HasValue ? lineDetails.TestMediumPipeId.Value : Guid.Empty,
                MDMT = lineDetails.MinimumDesignMetalTemperature,
                CorrosionAllowanceId = lineDetails.CorrosionAllowancePipeId.HasValue ? lineDetails.CorrosionAllowancePipeId.Value : Guid.Empty,
                XRayId = lineDetails.XrayPipeId.HasValue ? lineDetails.XrayPipeId.Value : Guid.Empty,
                NDECategoryId = lineDetails.NdeCategoryPipeId.HasValue ? lineDetails.NdeCategoryPipeId.Value : Guid.Empty,
                PWHTId = lineDetails.PostWeldHeatTreatmentId.HasValue ? lineDetails.PostWeldHeatTreatmentId.Value : Guid.Empty,
                StressAnalysisId = lineDetails.StressAnalysisId.HasValue ? lineDetails.StressAnalysisId.Value : Guid.Empty,
                InternalCoatingId = lineDetails.InternalCoatingLinerId.HasValue ? lineDetails.InternalCoatingLinerId.Value : Guid.Empty,
                CodeId = primaryOpMode != null ? primaryOpMode.CodeId.HasValue ? primaryOpMode.CodeId.Value : Guid.Empty : Guid.Empty,
                LineRevisionOperatingModeId = primaryOpMode != null ? primaryOpMode.Id : Guid.Empty,

                // CSA Pipelines
                FluidId = primaryOpMode.FluidId.HasValue ? primaryOpMode.FluidId.Value : Guid.Empty,
                FluidName = primaryOpMode != null ? primaryOpMode.Fluid != null ? primaryOpMode.Fluid.Name : string.Empty : string.Empty,
                CsaClassLocationId = primaryOpMode.CsaClassLocationId.HasValue ? primaryOpMode.CsaClassLocationId.Value : Guid.Empty,
                CsaHvpLvpId = primaryOpMode.CsaHvpLvpId.HasValue ? primaryOpMode.CsaHvpLvpId.Value : Guid.Empty,
                CsaClassLocationName = primaryOpMode != null ? primaryOpMode.CsaClassLocation != null ? primaryOpMode.CsaClassLocation.Name : string.Empty : string.Empty,
                CsaHvpLvpName = primaryOpMode != null ? primaryOpMode.CsaHvpLvp != null ? primaryOpMode.CsaHvpLvp.Name : string.Empty : string.Empty,
                PipeMaterialSpec = primaryOpMode != null ? primaryOpMode.PipeMaterialSpecification : string.Empty,
                HoopStressLevel = primaryOpMode != null ? primaryOpMode.HoopStressLevel.ToString() : string.Empty,
                //SourService = primaryOpMode != null ? primaryOpMode.IsSourService.ToString() : string.Empty,

                // Jacket Information
                //Jacket = Segment.TracingType != null ? Segment.TracingType.Name_dash_Description : string.Empty,
                JacketSizeNpsPipeId = lineDetails.SizeNpsAnnulusId.HasValue ? lineDetails.SizeNpsAnnulusId.Value : Guid.Empty,
                JacketDesignPressure = lineDetails.DesignPressureAnnulus,
                JacketUpsetPressure = lineDetails.UpsetPressureAnnulus,
                JacketDesignTempMin = lineDetails.DesignTemperatureMinimumAnnulus,
                JacketUpsetTemp = lineDetails.UpsetTemperatureAnnulus,
                JacketDesignTempMax = lineDetails.DesignTemperatureMaximumAnnulus,
                JacketScheduleId = lineDetails.ScheduleAnnulusId.HasValue ? lineDetails.ScheduleAnnulusId.Value : Guid.Empty,
                JacketCorrosionAllowanceId = lineDetails.CorrosionAllowanceAnnulusId.HasValue ? lineDetails.CorrosionAllowanceAnnulusId.Value : Guid.Empty,
                JacketWallThickness = lineDetails.WallThicknessAnnulus.ToString(),
                JacketXRayId = lineDetails.XrayAnnulusId.HasValue ? lineDetails.XrayAnnulusId.Value : Guid.Empty,
                JacketTestPressure = lineDetails.TestPressureAnnulus,
                JacketNDECategoryId = lineDetails.NdeCategoryAnnulusId.HasValue ? lineDetails.NdeCategoryAnnulusId.Value : Guid.Empty,
                JacketTestMediumId = lineDetails.TestMediumAnnulusId.HasValue ? lineDetails.TestMediumAnnulusId.Value : Guid.Empty,

                //Properties Tab
                Id = lineDetails.Id,
                CreatedBy = lineDetails.CreatedBy,
                CreatedOn = lineDetails.CreatedOn,
                ModifiedBy = lineDetails.ModifiedBy,
                ModifiedOn = lineDetails.ModifiedOn,
                IsMinimumInformationCompliance = lineDetails.RequiresMinimumInformation,
                IsActive = lineDetails.IsActive,
                ChildNumber = lineDetails.Line.ChildNumber,

                //Mode = lineDetails.LineRevisionOperatingModes,

                LineRevisionOperatingModes = allModes
                .Where(m => m.OperatingModeNumber != "1")
                .ToList(),

                // Additional UI elements
                IsCsa = primaryOpMode != null && primaryOpMode.Code != null && primaryOpMode.Code.IsCsa,

                //Segment Grid list
                LineRevisionSegment = Segments


            };

            

            return PartialView("_Update", Model);
        }

        // GET JSON for a single operating mode
        [HttpGet]
        public async Task<JsonResult> GetOperatingMode(Guid id)
        {
            var mode = await _lineRevisionOperatingModeService.GetById(id);
            if (mode == null) return Json(null);
            return Json(new
            {
                id = mode.Id,
                lineRevisionId = mode.LineRevisionId,
                operatingModeId = mode.OperatingModeId,
                operatingPressurePipe = mode.OperatingPressurePipe,
                operatingTemperaturePipe = mode.OperatingTemperaturePipe,
                lineRoutingFrom = mode.LineRoutingFrom,
                lineRoutingTo = mode.LineRoutingTo,
                notes = mode.Notes,
                codeId = mode.CodeId,
                fluidId = mode.FluidId,
                csaHvpLvpId = mode.CsaHvpLvpId,
                hoopStressLevel = mode.HoopStressLevel,
                csaClassLocationId = mode.CsaClassLocationId,
                pipeMaterialSpecification = mode.PipeMaterialSpecification,
                isSourService = mode.IsSourService,
                pressureProtectionId = mode.PressureProtectionId,
                isAbsaRegistration = mode.IsAbsaRegistration

            });
        }

        // Save (add or update) via AJAX
        [HttpPost]
        public async Task<JsonResult> SaveOperatingMode([FromBody] LineRevisionOperatingMode dto)
        {
            if (dto.Id == Guid.Empty)
            {
                var newEntity = new LineRevisionOperatingMode
                {
                    Id = Guid.NewGuid(),
                    LineRevisionId = dto.LineRevisionId,
                    OperatingModeNumber = "A",  // or assign properly
                    OperatingModeId = dto.OperatingModeId,
                    LineRoutingFrom = dto.LineRoutingFrom,
                    LineRoutingTo = dto.LineRoutingTo,
                    OperatingPressurePipe = dto.OperatingPressurePipe,
                    OperatingTemperaturePipe = dto.OperatingTemperaturePipe,
                    IsAbsaRegistration = dto.IsAbsaRegistration,
                    CodeId = dto.CodeId,
                    Notes = dto.Notes,
                    FluidId = dto.FluidId,
                    CsaHvpLvpId = dto.CsaHvpLvpId,
                    HoopStressLevel = dto.HoopStressLevel,
                    CreatedBy = _currentUser.FullName,
                    CreatedOn = DateTime.UtcNow
                };
                await _lineRevisionOperatingModeService.Add(newEntity);
            }
            else
            {
                var existing = await _lineRevisionOperatingModeService.GetById(dto.Id);
                existing.OperatingModeId = dto.OperatingModeId;
                existing.LineRoutingFrom = dto.LineRoutingFrom;
                //existing.LineRoutingTo = dto.LineRoutingTo;
                existing.OperatingPressurePipe = dto.OperatingPressurePipe;
                existing.OperatingTemperaturePipe = dto.OperatingTemperaturePipe;
                existing.PressureProtectionId = dto.PressureProtectionId;
                existing.IsAbsaRegistration = dto.IsAbsaRegistration;
                existing.CodeId = dto.CodeId;
                existing.Notes = dto.Notes;
                existing.FluidId = dto.FluidId;
                existing.CsaHvpLvpId = dto.CsaHvpLvpId;
                existing.HoopStressLevel = dto.HoopStressLevel;
                existing.ModifiedBy = _currentUser.FullName;
                existing.ModifiedOn = DateTime.UtcNow;
                await _lineRevisionOperatingModeService.Update(existing);
            }
            return Json(new { success = true });
        }

        // Delete via AJAX
        [HttpPost]
        public async Task<JsonResult> DeleteOperatingMode(Guid id)
        {
            var mode = await _lineRevisionOperatingModeService.GetById(id);
            if (mode != null)
                await _lineRevisionOperatingModeService.Remove(mode);
            return Json(new { success = true });
        }


        [HttpPost]
        public async Task<JsonResult> Update(LineCollectionViewModel model)
        {
            //if (!ModelState.IsValid)
            //    return Json(new { success = false, ErrorMessage = "Model is not valid" });

            var lineDetails = await _lineRevisionService.GetById(model.Id);
            if (lineDetails == null)
            {
                return Json(new { success = false, errorMessage = "Current Line Revision is null" });
            }

            LineRevisionOperatingMode primaryOpMode = null;
            if (lineDetails.LineRevisionOperatingModes.Any(x => x.OperatingModeNumber == "1"))
            {
                primaryOpMode = lineDetails.LineRevisionOperatingModes.Single(x => x.OperatingModeNumber == "1");
            }
            else
            {
                primaryOpMode = new LineRevisionOperatingMode();
                primaryOpMode.Id = Guid.NewGuid();
                primaryOpMode.OperatingModeNumber = "1";
                primaryOpMode.CreatedBy = _currentUser.FullName;
                primaryOpMode.CreatedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
                primaryOpMode.ModifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
                primaryOpMode.ModifiedBy = _currentUser.FullName;
                primaryOpMode.OperatingMode = await _operatingModeService.GetOperatingModeForPrimary();
                primaryOpMode.OperatingModeId = primaryOpMode.OperatingMode.Id;
                primaryOpMode.LineRevisionId = lineDetails.Id;
                await _lineRevisionOperatingModeService.Add(primaryOpMode);
            }
            // Update Primary Operating Mode Fields
            primaryOpMode.OperatingPressurePipe = model.OperatingPressurePipe?.Trim().ToUpper();
            primaryOpMode.OperatingTemperaturePipe = model.OperatingTemp?.Trim().ToUpper();
            primaryOpMode.Notes = model.Notes?.Trim();

            // Update LineRevision Fields
            lineDetails.AreaId = model.AreaId;
            // lineDetails.DesignPressurePipe = model.DesignPressure?.Trim().ToUpper();
            // lineDetails.DesignTemperatureMaximumPipe = model.DesignTempMax?.Trim().ToUpper();
            // lineDetails.DesignTemperatureMinimumPipe = model.DesignTempMin?.Trim().ToUpper();
            // lineDetails.ExpansionTemperature = model.ExpansionTemp?.Trim().ToUpper();
            // lineDetails.UpsetPressurePipe = model.UpsetPressure?.Trim().ToUpper();
            // lineDetails.UpsetTemperaturePipe = model.UpsetTemp?.Trim().ToUpper();
            // lineDetails.AreaId = model.AreaId;
            // lineDetails.PipeSpecificationId = model.PipeSpecificationId;
            // lineDetails.SizeNpsPipeId = model.SizeNpsPipeId;
            // //lineDetails.FluidPhaseId = model.FluidPhaseId;
            // //lineDetails.PressureProtectionId = model.PressureProtectionId;
            // lineDetails.SchedulePipeId = model.ScheduleId;
            // lineDetails.CorrosionAllowancePipeId = model.CorrosionAllowanceId;
            // lineDetails.StressAnalysisId = model.StressAnalysisId;
            // lineDetails.XrayPipeId = model.XRayId;
            // lineDetails.InternalCoatingLinerId = model.InternalCoatingId;
            // lineDetails.NdeCategoryPipeId = model.NDECategoryId;
            //// lineDetails.CodeId = model.CodeId;
            // lineDetails.TestMediumPipeId = model.TestMediumId;
            // lineDetails.PostWeldHeatTreatmentId = model.PWHTId;
            // //lineDetails.FluidId = model.FluidId;
            // //lineDetails.CsaHvpLvpId = model.CsaHvpLvpId;
            // //lineDetails.CsaClassLocationId = model.CsaClassLocationId;
            // // Jacket Information
            // lineDetails.SizeNpsAnnulusId = model.JacketSizeNpsPipeId;
            // lineDetails.DesignPressureAnnulus = model.JacketDesignPressure?.Trim().ToUpper();
            // lineDetails.UpsetPressureAnnulus = model.JacketUpsetPressure?.Trim().ToUpper();
            // lineDetails.DesignTemperatureMinimumAnnulus = model.JacketDesignTempMin?.Trim().ToUpper();
            // lineDetails.UpsetTemperatureAnnulus = model.JacketUpsetTemp?.Trim().ToUpper();
            // lineDetails.DesignTemperatureMaximumAnnulus = model.JacketDesignTempMax?.Trim().ToUpper();
            // lineDetails.ScheduleAnnulusId = model.JacketScheduleId;
            // lineDetails.CorrosionAllowanceAnnulusId = model.JacketCorrosionAllowanceId;
            // //lineDetails.WallThicknessAnnulus = model.JacketWallThickness?.Trim().ToUpper();
            // lineDetails.XrayAnnulusId = model.JacketXRayId;
            // lineDetails.TestPressureAnnulus = model.JacketTestPressure?.Trim().ToUpper();
            // lineDetails.NdeCategoryAnnulusId = model.JacketNDECategoryId;
            // lineDetails.TestMediumAnnulusId = model.JacketTestMediumId;

            lineDetails.ModifiedBy = _currentUser.FullName;
            lineDetails.ModifiedOn = DateTime.Now;

            await _lineRevisionService.Update(lineDetails);


            return Json(new
            {
                success = true,
                updatedRow = new
                {
                    id = lineDetails.Id,
                    modularId = lineDetails.ModularId
                }
            });
        }

        public async Task<IActionResult> IncludeExistingLines(Guid lineListRevisionId)
        {
            ViewBag.LineListRevisionId = lineListRevisionId;
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

            var lineListRevision = await _lineListRevisionService.GetById(lineListRevisionId);

            // If the LineListRevision exists and is valid, get the FacilityId
            Guid defaultFacilityId = Guid.Empty;
            if (lineListRevision?.EpProject?.CenovusProject?.Facility != null)
            {
                defaultFacilityId = lineListRevision.EpProject.CenovusProject.FacilityId;
            }
            else
            {
                // If there's no FacilityId, you can set a default or handle as needed
                defaultFacilityId = Guid.Empty;
            };

            var model = new SearchLineListViewModel
            {
                Commodities = commodities,
                Facilities = facilities,
                SelectedFacilityId = defaultFacilityId,
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
                LineLists = lineLists
            };

            return PartialView("_IncludeExistingLines", model);
        }

        [HttpPost]
        public async Task<IActionResult> SearchResult(SearchLineListViewModel model)
        {
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

            return Json(results);
        }
        //[HttpPost]
        //public async Task<IActionResult> SearchResult(SearchLineListViewModel model)
        //{
        //    var lines = (await _lineRevisionService.GetFilteredLineRevisions(
        //        model.SelectedFacilityId != Guid.Empty ? model.SelectedFacilityId : (Guid?)null,
        //        model.SelectedSpecificationId != Guid.Empty ? model.SelectedSpecificationId : (Guid?)null,
        //        model.SelectedLocationId != Guid.Empty ? model.SelectedLocationId : (Guid?)null,
        //        model.SelectedCommodityId != Guid.Empty ? model.SelectedCommodityId : (Guid?)null,
        //        model.SelectedAreaId != Guid.Empty ? model.SelectedAreaId : (Guid?)null,
        //        model.SelectedcenovusProjectId != Guid.Empty ? model.SelectedcenovusProjectId : (Guid?)null,
        //        model.SelectedEPProjectId != Guid.Empty ? model.SelectedEPProjectId : (Guid?)null,
        //        model.SelectedPipeSpecificationId != Guid.Empty ? model.SelectedPipeSpecificationId : (Guid?)null,
        //        model.SelectedLineStatusId != Guid.Empty ? model.SelectedLineStatusId : (Guid?)null,
        //        model.ShowDrafts,
        //        model.ShowOnlyActive,
        //        model.SelectedDocumentNumberId,
        //        model.SelectedModularID,
        //        model.SelectedSequenceNumber != string.Empty ? model.SelectedSequenceNumber : (string?)null,
        //        model.SelectedProjectTypeId != Guid.Empty ? model.SelectedProjectTypeId : (Guid?)null
        //    )).ToList();

        //    var results = new List<LineResultDto>();
        //    //var allLines = _lineService.GetAll().Result.ToList();
        //    //var parentLineLookup = new HashSet<(Guid LocationId, Guid CommodityId, string SequenceNumber)>(
        //    //                                allLines.Where(c => c.ChildNumber != 0) // Filter parent lines
        //    //                                          .Select(c => (c.LocationId, c.CommodityId, c.SequenceNumber))
        //    //                            );
        //    var parentLineLookup = await _lineService.GetParentLineLookupAsync();

        //    foreach (var p in lines)
        //    {
        //        var resultDto = new LineResultDto();

        //        resultDto.Id = p.Id;
        //        resultDto.AreaName = p.Area?.Name ?? "";
        //        resultDto.CenovusProjectName = p.LineListRevision.EpProject?.CenovusProject?.Name ?? "";
        //        resultDto.ModularId = p.Line.ModularId;
        //        resultDto.DocumentNumber = p.LineListRevision?.LineListDocumentNumber;
        //        resultDto.DocumentRevision = p.Revision;
        //        resultDto.DocumentLineListRevision = p.LineListRevision?.DocumentRevision;
        //        resultDto.LineListStatusName = p.LineListRevision?.LineListStatus?.Name ?? "";
        //        resultDto.LineStatusName = p.LineStatus?.Name ?? "";
        //        resultDto.SpecificationName = p.Specification?.Name;
        //        resultDto.CreatedOn = p.CreatedOn;
        //        resultDto.LocationName = p.Line.Location?.Name;
        //        resultDto.CommoditiyName = p.Line.Commodity.Name ?? "";
        //        resultDto.PipeSpecificationName = p.PipeSpecification?.Name ?? "";
        //        resultDto.ParentChild = (p.Line.ChildNumber != 0) ? "C" :
        //            (parentLineLookup.Contains((p.Line.LocationId, p.Line.CommodityId, p.Line.SequenceNumber))) ? "p" : string.Empty;
        //        resultDto.SizeNpsName = p.SizeNpsPipe?.Name ?? "";
        //        resultDto.IsHardRevision = p.LineListRevision.LineListStatus?.IsHardRevision ?? false;
        //        resultDto.IsIssued = p.LineListRevision.LineListStatus?.IsIssuedOfId != null;
        //        resultDto.SequenceNumber = p.Line.SequenceNumber;


        //        results.Add(resultDto);
        //    }

        //    return Json(results);
        //}

        [HttpPost]
        public async Task<IActionResult> IncludeSelectedLines([FromBody] IncludeExistingLinesModel data)
        {
            try
            {
                await LineRules.IncludeExistingLines(
                    data.ExistingLineIds,
                    data.LineListRevisionId,
                    data.UserName,
                    data.IsReferenceLine,
                    _lineRevisionService,
                    _lineListRevisionService,
                    _lineStatusService,
                    _lineListStatusService,
                    _lineRevisionSegmentService,
                    _lineRevisionOperatingModeService,
                    _lineService
                );

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while including selected lines.", details = ex.Message });
            }
        }


        [HttpGet]
        public async Task<IActionResult> ReserveLines(Guid lineListRevisionId)
        {
            ViewBag.LineListRevisionId = lineListRevisionId;
            var cacheKey = $"ReserveLines_{lineListRevisionId}";
            if (!_memoryCache.TryGetValue(cacheKey, out ReserveLinesViewModel model))
            {
                _logger.LogInformation("Cache miss for ReserveLinesViewModel, LineListRevisionId: {LineListRevisionId}", lineListRevisionId);

                var lineListRevision = await _lineListRevisionService.GetById(lineListRevisionId);
                if (lineListRevision == null)
                {
                    _logger.LogWarning("LineListRevision not found for ID: {LineListRevisionId}", lineListRevisionId);
                    return NotFound("LineListRevision not found.");
                }

                var specification = (await _specificationService.GetAll()).FirstOrDefault(s => s.Name == "TR");
                if (specification == null)
                {
                    _logger.LogError("Specification 'TR' not found for LineListRevisionId: {LineListRevisionId}", lineListRevisionId);
                    return BadRequest("Specification 'TR' not found.");
                }

                var facilityId = lineListRevision.EpProject?.CenovusProject?.FacilityId;
                if (!facilityId.HasValue)
                {
                    _logger.LogWarning("Facility not found for LineListRevisionId: {LineListRevisionId}", lineListRevisionId);
                    return BadRequest("Facility not found.");
                }

                var locations = (await _locationService.GetAll())
                    .Where(l => l.FacilityId == facilityId && l.IsActive)
                    .OrderBy(l => l.SortOrder)
                    .ToList();

                var commodities = (await _commodityService.GetAll())
                    .Where(c => c.SpecificationId == specification.Id && c.IsActive)
                    .OrderBy(c => c.SortOrder)
                    .ToList();

                model = new ReserveLinesViewModel
                {
                    LineListRevisionId = lineListRevisionId,
                   DocumentNumber = lineListRevision.LineListDocumentNumber,
                    DocumentRevision = lineListRevision.DocumentRevision,
                    LineListStatusName = lineListRevision.LineListStatus?.Name,
                    SpecificationId = specification.Id,
                    SpecificationName = specification.Name,
                    Locations = locations,
                    Commodities = commodities,
                    StartingLineSequence = 1,
                    NumberOfLines = 1,
                    Contiguous = true
                };

                _memoryCache.Set(cacheKey, model, TimeSpan.FromMinutes(10));
                _logger.LogInformation("Cached ReserveLinesViewModel for LineListRevisionId: {LineListRevisionId}", lineListRevisionId);
            }
            else
            {
                _logger.LogInformation("Cache hit for ReserveLinesViewModel, LineListRevisionId: {LineListRevisionId}", lineListRevisionId);
            }

            return PartialView("_ReserveLines", model);
        }

        [HttpPost]
        public async Task<IActionResult> ReserveNewLines(ReserveLinesViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    return Json(new { success = false, errorMessage = "Validation failed: " + string.Join(", ", errors) });
                }

                var lineListRevision = await _lineListRevisionService.GetById((Guid)model.LineListRevisionId);
                if (lineListRevision == null)
                {
                    return Json(new { success = false, errorMessage = "Line list revision not found." });
                }

                // Validate inputs
                if (!model.LocationId.HasValue || !model.CommodityId.HasValue || model.NumberOfLines < 1 || string.IsNullOrEmpty(model.SpecificationName))
                {
                    return Json(new { success = false, errorMessage = "Location, Commodity, Number of Lines are required, and SpecificationName are required." });
                }

                int startingLineSequence = model.StartingLineSequence > 0 ? model.StartingLineSequence : 1;
                int numberOfLines = model.NumberOfLines;

                // Check sequence number range
                if (startingLineSequence + numberOfLines - 1 > 9999 || startingLineSequence > 9999 || numberOfLines > 9999)
                {
                    return Json(new { success = false, errorMessage = "Sequence numbers cannot exceed 9999." });
                }

                // Get reserved sequence numbers
                var sequenceNums = await GetReservedSequenceList(
                    model.LocationId.Value,
                    model.CommodityId.Value,
                    startingLineSequence,
                    numberOfLines,
                    model.Contiguous,
                    model.OverrideSequence);

                bool matchSequence = true;
                if (sequenceNums.Any() && startingLineSequence != sequenceNums[0] && !model.OverrideSequence)
                {
                    matchSequence = false;
                }

                if (sequenceNums.Any() && matchSequence)
                {
                    var newLineIds = new List<Guid>();
                    var currentUser = User.Identity?.Name ?? "System";
                    var modifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

                    // Fetch default entities
                    var primaryOpMode = (await _operatingModeService.GetAll()).FirstOrDefault(m => m.Name.ToUpper() == "P");
                    if (primaryOpMode == null)
                    {
                        return Json(new { success = false, errorMessage = "Primary operating mode not found." });
                    }

                    var specification = await _specificationService.GetById(model.SpecificationId);
                    if (specification == null)
                    {
                        return Json(new { success = false, errorMessage = "Specification not found." });
                    }

                    var lineStatus = (await _lineStatusService.GetAll()).FirstOrDefault(x => x.IsDefaultForReservation);
                    if (lineStatus == null)
                    {
                        return Json(new { success = false, errorMessage = "Default line status for reservation not found." });
                    }

                    var segmentType = (await _segmentTypeService.GetAll()).FirstOrDefault(m => m.Name == "I/S");
                    if (segmentType == null)
                    {
                        return Json(new { success = false, errorMessage = "Segment type 'I/S' not found." });
                    }

                    // Double-check that the SegmentType exists in the database
                    var segmentTypeExists = await _segmentTypeService.GetById(segmentType.Id);
                    if (segmentTypeExists == null)
                    {
                        return Json(new { success = false, errorMessage = $"Segment type with ID {segmentType.Id} does not exist in the database." });
                    }

                    var location = await _locationService.GetById(model.LocationId.Value);
                    var commodity = await _commodityService.GetById(model.CommodityId.Value);
                    var lineNumberPrefix = $"{location.Name}-{commodity.Name}-*-*-";

                    // Determine AreaId if the location matches the line list revision's area
                    Guid? areaId = null;
                    var areas = (await _areaService.GetAll()).Where(a => a.LocationId == model.LocationId.Value).ToList();
                    if (lineListRevision.Area != null && areas.Any(a => a.Id == lineListRevision.AreaId))
                    {
                        areaId = lineListRevision.AreaId;
                    }

                    // Create new lines
                    foreach (var seqNum in sequenceNums)
                    {
                        var line = new Line
                        {
                            Id = Guid.NewGuid(),
                            CommodityId = model.CommodityId.Value,
                            LocationId = model.LocationId.Value,
                            SequenceNumber = seqNum.ToString("D4"), // Zero-pad to 4 digits
                            ChildNumber = 0,
                            CreatedBy = currentUser,
                            CreatedOn = modifiedOn,
                            ModifiedBy = currentUser,
                            ModifiedOn = modifiedOn
                        };

                        var lineRevision = new LineRevision
                        {
                            Id = Guid.NewGuid(),
                            LineId = line.Id,
                            LineListRevisionId = model.LineListRevisionId,
                            IsActive = true,
                            IsCheckedOut = false,
                            RequiresMinimumInformation = true,
                            IsReferenceLine = false,
                            CreatedBy = currentUser,
                            CreatedOn = modifiedOn,
                            ModifiedBy = currentUser,
                            ModifiedOn = modifiedOn,
                            LineStatusId = lineStatus.Id,
                            SpecificationId = specification.Id,
                            AreaId = areaId,
                            Revision = lineListRevision.DocumentRevision,
                            ValidationState = (int)LineRevisionHardValidationState.Fail
                        };

                        var lineSegment = new LineRevisionSegment
                        {
                            Id = Guid.NewGuid(),
                            SegmentNumber = "1",
                            LineRevisionId = lineRevision.Id,
                            SegmentTypeId = segmentType.Id,
                            CreatedBy = currentUser,
                            CreatedOn = modifiedOn,
                            ModifiedBy = currentUser,
                            ModifiedOn = modifiedOn
                        };

                        // Double-check that LineRevisionId is set
                        if (lineSegment.LineRevisionId == Guid.Empty)
                        {
                            throw new InvalidOperationException("LineRevisionId cannot be null for LineRevisionSegment.");
                        }

                        var opMode = new LineRevisionOperatingMode
                        {
                            Id = Guid.NewGuid(),
                            OperatingModeNumber = "1",
                            OperatingModeId = primaryOpMode.Id,
                            LineRevisionId = lineRevision.Id,
                            CreatedBy = currentUser,
                            CreatedOn = modifiedOn,
                            ModifiedBy = currentUser,
                            ModifiedOn = modifiedOn
                        };

                        lineRevision.LineRevisionSegments = new List<LineRevisionSegment> { lineSegment };
                        lineRevision.LineRevisionOperatingModes = new List<LineRevisionOperatingMode> { opMode };
                        line.LineRevisions = new List<LineRevision> { lineRevision };
                        lineRevision.LineNumber = $"{lineNumberPrefix}{line.SequenceNumber}";

                        await _lineService.Add(line);
                        newLineIds.Add(lineRevision.Id);

                        // Update line number (simulating LineNumberGenerator.Evaluate)
                        lineRevision.LineNumber = await GenerateLineNumber(lineRevision);
                        await _lineService.Update(line);
                    }

                    var successMessage = $"Lines for this line list have been reserved successfully from {sequenceNums[0]} to {sequenceNums.Last()}";
                    return Json(new { success = true, message = successMessage });
                }
                else
                {
                    return Json(new { success = false, sequenceOverlap = true });
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                var innerException = ex.InnerException;
                while (innerException != null)
                {
                    errorMessage += $" | Inner: {innerException.Message}";
                    innerException = innerException.InnerException;
                }
                Console.WriteLine($"Error reserving lines: {errorMessage}");
                return Json(new { success = false, errorMessage = $"Error reserving lines: {errorMessage}" });
            }
        }


        //[HttpPost]
        ////[Authorize(Roles = "LL-ANO-EP-TQA,APP-CLO-LL-TQA-ADM-TQA,RSV")]
        //public async Task<IActionResult> ReserveNewLines(ReserveLinesViewModel model)
        //{
        //    using var transaction = await _dbContext.Database.BeginTransactionAsync();
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        //            _logger.LogWarning("Model validation failed for ReserveNewLines, LineListRevisionId: {LineListRevisionId}, Errors: {Errors}",
        //                model.LineListRevisionId, string.Join("; ", errors));
        //            return Json(new { success = false, errorMessage = string.Join("; ", errors) });
        //        }

        //        var lineListRevision = await _lineListRevisionService.GetById((Guid)model.LineListRevisionId);


        //        if (lineListRevision == null)
        //        {
        //            _logger.LogWarning("LineListRevision not found for ID: {LineListRevisionId}", model.LineListRevisionId);
        //            return Json(new { success = false, errorMessage = "LineListRevision not found." });
        //        }

        //        var specification = await _specificationService.GetById(model.SpecificationId);
        //        if (specification == null || specification.Name != "TR")
        //        {
        //            _logger.LogWarning("Invalid specification ID: {SpecificationId} for LineListRevisionId: {LineListRevisionId}",
        //                model.SpecificationId, model.LineListRevisionId);
        //            return Json(new { success = false, errorMessage = "Invalid specification." });
        //        }

        //        var primaryOpMode = (await _operatingModeService.GetAll()).FirstOrDefault(om => om.Name == "P");
        //        if (primaryOpMode == null)
        //        {
        //            _logger.LogError("Operating mode 'P' not found for LineListRevisionId: {LineListRevisionId}", model.LineListRevisionId);
        //            return Json(new { success = false, errorMessage = "Operating mode 'P' not found." });
        //        }

        //        var segmentType = (await _segmentTypeService.GetAll()).FirstOrDefault(st => st.Name == "I/S");
        //        if (segmentType == null)
        //        {
        //            _logger.LogError("Segment type 'I/S' not found for LineListRevisionId: {LineListRevisionId}", model.LineListRevisionId);
        //            return Json(new { success = false, errorMessage = "Segment type 'I/S' not found." });
        //        }

        //        var location = await _locationService.GetById(model.LocationId.Value);
        //        if (location == null)
        //        {
        //            _logger.LogWarning("Location not found for ID: {LocationId}, LineListRevisionId: {LineListRevisionId}",
        //                model.LocationId, model.LineListRevisionId);
        //            return Json(new { success = false, errorMessage = "Invalid location." });
        //        }

        //        var lineStatus = (await _lineStatusService.GetAll()).FirstOrDefault(x => x.IsDefaultForReservation);
        //        if (lineStatus == null)
        //        {
        //            _logger.LogError("Default line status for reservation not found.");
        //            return Json(new { success = false, errorMessage = "Default line status for reservation not found." });
        //        }
        //        var commodity = await _commodityService.GetById(model.CommodityId.Value);
        //        if (commodity == null)
        //        {
        //            _logger.LogWarning("Commodity not found for ID: {CommodityId}, LineListRevisionId: {LineListRevisionId}",
        //                model.CommodityId, model.LineListRevisionId);
        //            return Json(new { success = false, errorMessage = "Invalid commodity." });
        //        }


        //        int startingLineSequence = model.StartingLineSequence > 0 ? model.StartingLineSequence : 1;
        //        int numberOfLines = model.NumberOfLines;

        //        if (startingLineSequence + numberOfLines - 1 > 9999 || startingLineSequence > 9999)
        //        {
        //            _logger.LogWarning("Sequence numbers exceed limit (9999) for LineListRevisionId: {LineListRevisionId}", model.LineListRevisionId);
        //            return Json(new
        //            {
        //                success = false,
        //                sequenceOverlap = true,
        //                validationMsgExists = "The sequence numbers requested exceed the maximum limit (9999)."
        //            });
        //        }

        //        var sequenceNums = await GetReservedSequenceList(
        //            model.LocationId,
        //            model.CommodityId,
        //            startingLineSequence,
        //            numberOfLines,
        //            model.Contiguous,
        //            model.OverrideSequence);

        //        var matchSequence = sequenceNums.Any() && sequenceNums.Length == numberOfLines &&
        //                            (!model.Contiguous || (sequenceNums.Max() - sequenceNums.Min() + 1 == sequenceNums.Length)) &&
        //                            (model.OverrideSequence || sequenceNums[0] == startingLineSequence);

        //        if (!sequenceNums.Any() || !matchSequence)
        //        {
        //            _logger.LogWarning("Sequence overlap or insufficient sequences for LocationId: {LocationId}, CommodityId: {CommodityId}, StartingLineSequence: {StartingLineSequence}, NumberOfLines: {NumberOfLines}, Contiguous: {Contiguous}, LineListRevisionId: {LineListRevisionId}",
        //                model.LocationId, model.CommodityId, startingLineSequence, numberOfLines, model.Contiguous, model.LineListRevisionId);
        //            return Json(new
        //            {
        //                success = false,
        //                sequenceOverlap = true,
        //                validationMsgExists = "The sequence numbers requested are not available. Please select override to use the next available sequences or choose a different starting sequence."
        //            });
        //        }

        //        var newLineIds = new List<Guid>();
        //        var currentUser = _currentUser.FullName ?? "System";
        //        var modifiedOn = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
        //        var batchSize = 100;
        //        var linesToAdd = new List<Line>();
        //        var lineNumberPrefix = $"{location.Name}-{commodity.Name}-*-*-";

        //        // Determine AreaId
        //        Guid? areaId = null;
        //        var areas = (await _areaService.GetAll()).Where(a => a.LocationId == model.LocationId.Value).ToList();
        //        if (lineListRevision.AreaId.HasValue && areas.Any(a => a.Id == lineListRevision.AreaId))
        //        {
        //            areaId = lineListRevision.AreaId;
        //        }

        //        _logger.LogInformation("Starting line creation for {NumberOfLines} lines, LineListRevisionId: {LineListRevisionId}",
        //            numberOfLines, model.LineListRevisionId);

        //        for (int i = 0; i < sequenceNums.Length; i++)
        //        {
        //            var seqNum = sequenceNums[i].ToString("D4");
        //            var line = new Line
        //            {
        //                Id = Guid.NewGuid(),
        //                LocationId = model.LocationId.Value,
        //                CommodityId = model.CommodityId.Value,
        //                SequenceNumber = seqNum,
        //                ChildNumber = await _lineService.GetNextChildNumber(model.LocationId.Value, model.CommodityId.Value, seqNum),
        //                CreatedBy = currentUser,
        //                CreatedOn = modifiedOn,
        //                ModifiedBy = currentUser,
        //                ModifiedOn = modifiedOn
        //            };

        //            var lineRevision = new LineRevision
        //            {
        //                Id = Guid.NewGuid(),
        //                LineId = line.Id,
        //                LineListRevisionId = model.LineListRevisionId,
        //                SpecificationId = model.SpecificationId,
        //                AreaId = areaId,
        //                LineStatusId = lineStatus.Id,
        //                IsActive = true,
        //                IsCheckedOut = false,
        //                RequiresMinimumInformation = true,
        //                CreatedBy = currentUser,
        //                CreatedOn = modifiedOn,
        //                ModifiedBy = currentUser,
        //                ModifiedOn = modifiedOn,
        //                LineNumber = $"{lineNumberPrefix}{seqNum}",
        //                Revision = lineListRevision.DocumentRevision
        //            };

        //            var lineSegment = new LineRevisionSegment
        //            {
        //                Id = Guid.NewGuid(),
        //                LineRevisionId = lineRevision.Id,
        //                SegmentTypeId = segmentType.Id,
        //                SegmentNumber = "1",
        //                CreatedBy = currentUser,
        //                CreatedOn = modifiedOn,
        //                ModifiedBy = currentUser,
        //                ModifiedOn = modifiedOn
        //            };

        //            var opMode = new LineRevisionOperatingMode
        //            {
        //                Id = Guid.NewGuid(),
        //                LineRevisionId = lineRevision.Id,
        //                OperatingModeId = primaryOpMode.Id,
        //                OperatingModeNumber = "1",
        //                CreatedBy = currentUser,
        //                CreatedOn = modifiedOn,
        //                ModifiedBy = currentUser,
        //                ModifiedOn = modifiedOn
        //            };

        //            lineRevision.LineRevisionSegments = new List<LineRevisionSegment> { lineSegment };
        //            lineRevision.LineRevisionOperatingModes = new List<LineRevisionOperatingMode> { opMode };
        //            line.LineRevisions = new List<LineRevision> { lineRevision };

        //            linesToAdd.Add(line);

        //            if (linesToAdd.Count >= batchSize || i == sequenceNums.Length - 1)
        //            {
        //                foreach (var lineToAdd in linesToAdd)
        //                {
        //                    await _lineService.Add(lineToAdd);
        //                }
        //                newLineIds.AddRange(linesToAdd.Select(l => l.LineRevisions.First().Id));
        //                await _dbContext.SaveChangesAsync();
        //                _logger.LogInformation("Saved batch of {BatchCount} lines for LineListRevisionId: {LineListRevisionId}",
        //                    linesToAdd.Count, model.LineListRevisionId);
        //                linesToAdd.Clear();
        //            }
        //        }

        //        await transaction.CommitAsync();

        //        var successMessage = $"Lines reserved from {sequenceNums[0]:D4} to {sequenceNums.Last():D4}";
        //        _logger.LogInformation("Successfully reserved {NumberOfLines} lines for LineListRevisionId: {LineListRevisionId}: {Message}",
        //            sequenceNums.Length, model.LineListRevisionId, successMessage);

        //        return Json(new { success = true, message = successMessage });
        //    }
        //    catch (DbUpdateException ex) when (ex.InnerException?.Message.Contains("unique constraint") ?? false)
        //    {
        //        await transaction.RollbackAsync();
        //        _logger.LogError(ex, "Unique constraint violation while reserving lines for LineListRevisionId: {LineListRevisionId}", model.LineListRevisionId);
        //        return Json(new
        //        {
        //            success = false,
        //            errorMessage = "Sequence numbers are already in use. Please try overriding or selecting a different range."
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        _logger.LogError(ex, "Error reserving lines for LineListRevisionId: {LineListRevisionId}", model.LineListRevisionId);
        //        return Json(new
        //        {
        //            success = false,
        //            errorMessage = "An error occurred while reserving lines. Please contact support."
        //        });
        //    }
        //}
        private async Task<int[]> GetReservedSequenceList(Guid? locationId, Guid? commodityId, int startingLineSequence, int numberOfLines, bool contiguous, bool overrideSequence)
        {
            if (!locationId.HasValue || !commodityId.HasValue)
            {
                return new int[0]; // Return empty array if inputs are invalid
            }
            // Simulate LineListRules.GetReservedSequenceList
            var existingLines = await _lineService.GetByLocationAndCommodity(locationId.Value, commodityId.Value);
            var existingSequences = existingLines.Select(l => int.Parse(l.SequenceNumber)).OrderBy(s => s).ToList();

            var sequenceNums = new List<int>();
            int start = overrideSequence ? existingSequences.Any() ? existingSequences.Max() + 1 : startingLineSequence : startingLineSequence;

            if (contiguous)
            {
                // Find the first contiguous block of sequence numbers
                while (sequenceNums.Count < numberOfLines)
                {
                    bool canUseBlock = true;
                    for (int i = start; i < start + numberOfLines; i++)
                    {
                        if (existingSequences.Contains(i))
                        {
                            canUseBlock = false;
                            break;
                        }
                    }

                    if (canUseBlock)
                    {
                        for (int i = start; i < start + numberOfLines; i++)
                        {
                            sequenceNums.Add(i);
                        }
                    }
                    else
                    {
                        start++;
                    }

                    if (start + numberOfLines > 9999)
                    {
                        return new int[0]; // No contiguous block available
                    }
                }
            }
            else
            {
                // Non-contiguous: find any available sequence numbers
                int current = start;
                while (sequenceNums.Count < numberOfLines && current <= 9999)
                {
                    if (!existingSequences.Contains(current))
                    {
                        sequenceNums.Add(current);
                    }
                    current++;
                }
            }

            return sequenceNums.ToArray();
        }
        private async Task<string> GenerateLineNumber(LineRevision lineRevision)
        {
            // Simulate LineNumberGenerator.Evaluate
            var line = await _lineService.GetById(lineRevision.LineId);
            var location = await _locationService.GetById(line.LocationId);
            var commodity = await _commodityService.GetById(line.CommodityId);
            return $"{location.Name}-{commodity.Name}-*-*-{line.SequenceNumber}";
        }
        [HttpGet]
        public async Task<IActionResult> ConcurrentEngineeringReport(Guid lineListRevisionId)
        {
            try
            {
                if (lineListRevisionId == Guid.Empty)
                {
                    return BadRequest(new { error = "Line List Revision ID is required." });
                }
                var lineListRevision = await _lineListRevisionService.GetById(lineListRevisionId);
                if (lineListRevision == null)
                {
                    return NotFound(new { error = "Line List Revision not found." });
                }

                var projectId = lineListRevision.EpProjectId;
                ViewBag.ProjectId = projectId;

                var userIsCenovusAdmin = User.IsInRole("CenovusAdmin");
                var userIsEpLeadEng = projectId != Guid.Empty && User.Claims.Any(c => c.Type == "EpLeadEng" && c.Value == projectId.ToString());
                var userIsEpDataEntry = User.Claims.Any(c => c.Type == "EpDataEntry" && c.Value == projectId.ToString());

                //if (!userIsCenovusAdmin && !userIsEpLeadEng && !userIsEpDataEntry)
                //{
                //    return Unauthorized(new { error = "You do not have permission to view this report." });
                //}

                var Model = new ConcurrentEngineeringReportViewModel
                {
                    LineListRevisionId = lineListRevisionId,
                    Title = $"Multiple Active Revisions for Lines in Line List: {lineListRevision.LineListDocumentNumber} (Rev: {lineListRevision.DocumentRevision})"
                };

                return View("ConcurrentEngineeringReport", Model);
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                Console.WriteLine($"Error in ConcurrentEngineeringReport: {ex.Message}");
                return StatusCode(500, new { error = "An error occurred while loading the report." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetConcurrentEngineeringLinesForLineList(Guid lineListRevisionId)
        {
            try
            {
                //var draw = Request.Form["draw"].FirstOrDefault();
                //var start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
                //var length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
                //var searchValue = Request.Form["search[value]"].FirstOrDefault();
                //var lineListRevisionId = Guid.Parse(Request.Form["lineListRevisionId"]);
                if (lineListRevisionId == Guid.Empty)
                {
                    return BadRequest(new { error = "Line List Revision ID is required." });
                }
                // Check permissions
                var lineListRevision = await _lineListRevisionService.GetById(lineListRevisionId);
                if (lineListRevision == null)
                {
                    return Json(new { error = "Line List not found." });
                }

                var projectId = lineListRevision.EpProjectId;
                var userIsCenovusAdmin = User.IsInRole("CenovusAdmin");
                var userIsEpLeadEng = projectId != Guid.Empty && User.Claims.Any(c => c.Type == "EpLeadEng" && c.Value == projectId.ToString());
                var userIsEpDataEntry = User.Claims.Any(c => c.Type == "EpDataEntry" && c.Value == projectId.ToString());

                if (!userIsCenovusAdmin && !userIsEpLeadEng && !userIsEpDataEntry)
                {
                    return Json(new { error = "Unauthorized: You do not have permission to view this report." });
                }

                var lines = await BuildConcurrentEngineeringQueryForLineList(lineListRevisionId);

                return Json(lines);
            }
            catch (Exception ex)
            {
                return Json(new { error = $"Error loading Concurrent Engineering lines: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> GetMechanicalDefaults(Guid pipeSpecificationId, Guid sizeNpsPipeId)
        {

            var pipeSpec = await _dbContext.PipeSpecifications
                                   .FirstOrDefaultAsync(ps => ps.Id == pipeSpecificationId);
            if (pipeSpec == null)
                return Json(new { error = "Pipe specification not found." });

           
            var scheduleDefault = await _dbContext.ScheduleDefaults
                .Where(sd => sd.PipeSpecificationId == pipeSpecificationId
                          && sd.SizeNpsId == sizeNpsPipeId)
                .Select(sd => sd.ScheduleId)
                .FirstOrDefaultAsync();

         
            return Json(new
            {
                corrosionAllowanceId = pipeSpec.CorrosionAllowanceId,
                xrayId = pipeSpec.XrayId,
                ndeCategoryId = pipeSpec.NdeCategoryId,
                scheduleId = scheduleDefault
            });
        }

        [HttpPost]
        public async Task<JsonResult> GetInsulationTracingDefaults(
        Guid lineListRevisionId,
        Guid sizeNpsId,
        Guid insulationTypeId,
        Guid tracingDesignId,
        Guid insulationMaterialId,
        string operatingTemp)
        {
            // get the EPProjectId for this line list
            var llr = await _lineListRevisionService.GetById(lineListRevisionId);
            if (llr == null) return Json(new { error = "Line list not found." });
            var projectId = llr.EpProjectId;

            // replicate your old EF logic:
            var insDefault = _dbContext.EpProjectInsulationDefaults
                .OrderBy(d => d.SortOrder)
                .FirstOrDefault(d =>
                    d.EpProjectId == projectId
                    && d.InsulationTypeId == insulationTypeId
                    && d.InsulationMaterialId == insulationMaterialId
                    && d.TracingTypeId == tracingDesignId);

            if (insDefault == null) return Json(new { error = "No defaults found." });

            // pick the right row
            var row = insDefault.Rows.FirstOrDefault(r => r.SizeNpsId == sizeNpsId);
            if (row == null) return Json(new { error = "No defaults for that size." });

            // pick the right column based on operatingTemp
            EpProjectInsulationDefaultColumn col = null;
            if (double.TryParse(operatingTemp, out var t))
            {
                col = insDefault.Columns
                    .FirstOrDefault(c => c.MinOperatingTemperature <= t
                                      && c.MaxOperatingTemperature >= t);
            }
            if (col == null) return Json(new { error = "No defaults for that temperature." });

            // pull the detail
            var detail = _dbContext.EpProjectInsulationDefaultDetails
                .FirstOrDefault(d =>
                    d.EpProjectInsulationDefaultRowId == row.Id
                    && d.EpProjectInsulationDefaultColumnId == col.Id);

            if (detail == null) return Json(new { error = "No detail record found." });

            return Json(new
            {
                insulationThicknessId = detail.InsulationThicknessId,
                tracingDesignNumberOfTracersId = detail.TracingDesignNumberOfTracersId,
                tracingHoldTemp = /* Earlier this came from the TracingType entity, not detail */
                    _dbContext.TracingTypes
                              .Find(tracingDesignId)?
                              .Temperature ?? string.Empty
            });
        }


        private async Task<List<ConcurrentEngineeringLineResultDto>> BuildConcurrentEngineeringQueryForLineList(Guid lineListRevisionId)
        {
            var lineRevisions = await _lineRevisionService.GetAll();
            var lineListRevisions = await _lineListRevisionService.GetAll();
            var lineLists = await _lineListModelService.GetAll();

            var epProjects = await _epProjectService.GetAll();
            var epProjectDict = epProjects.ToDictionary(ep => ep.Id, ep => ep);

            var targetLineRevisions = lineRevisions.Where(lr => lr.LineListRevisionId == lineListRevisionId && lr.IsActive).ToList();
            var targetLineIds = targetLineRevisions.Select(lr => lr.LineId).Distinct().ToList();

            var query = from lr in lineRevisions
                        join llr in lineListRevisions on lr.LineListRevisionId equals llr.Id
                        join ll in lineLists on llr.LineListId equals ll.Id
                        where lr.IsActive && // Only active revisions
                              lr.LineListRevisionId != lineListRevisionId && // Exclude the current Line List
                              targetLineIds.Contains(lr.LineId) // Lines that are on the target Line List
                        let epProject = epProjectDict.ContainsKey(llr.EpProjectId) ? epProjectDict[llr.EpProjectId] : null
                        select new ConcurrentEngineeringLineResultDto
                        {
                            Id = lr.Id,
                            LineId = lr.LineId,
                            LineNumber = lr.LineNumber,
                            LineListRevisionId = llr.Id,
                            EpProjectId = llr.EpProjectId,
                            FacilityId = llr.LocationId ?? Guid.Empty,
                            ChildNumber = lr.Line != null ? lr.Line.ChildNumber : 0,
                            Location = llr.Location != null ? llr.Location.Name : "",
                            Commodity = lr.Line != null && lr.Line.Commodity != null ? lr.Line.Commodity.Name : "",
                            SequenceNumber = lr.Line != null ? lr.Line.SequenceNumber : "",
                            DocumentNumber = llr.LineListDocumentNumber,
                            LineListStatus = llr.LineListStatus != null ? llr.LineListStatus.Name : "",
                            DocumentRevision = llr.DocumentRevision,
                            EP = epProject != null ? epProject.Name : "",
                            LineRevision = lr.Revision,
                            LineStatus = lr.LineStatus != null ? lr.LineStatus.Name : "",
                            IsActive = lr.IsActive,
                            IsActiveText = lr.IsActive ? "Yes" : "No",
                            IsDraft = llr.LineListStatus != null && llr.LineListStatus.IsDraftOfId.HasValue,
                            ParentChild = lr.Line != null && lr.Line.ChildNumber > 0 ? "C" : "P",
                            LocationId = llr.LocationId ?? Guid.Empty,
                            CommodityId = lr.Line != null ? lr.Line.CommodityId : Guid.Empty,
                            LineListId = llr.LineListId,
                            ShortLineNumber = lr.Line != null ? $"{lr.Line.Location.Name}-{lr.Line.Commodity.Name}-{lr.Line.SequenceNumber}" : "",
                            Specification = lr.Specification != null ? lr.Specification.Name : "",
                            AsBuiltCount = 0, // Not used in specific mode
                            IssuedOn = llr.IssuedOn,
                            CreatedBy = lr.CreatedBy,
                            CreatedOn = lr.CreatedOn,
                            ModifiedBy = lr.ModifiedBy,
                            ModifiedOn = lr.ModifiedOn,
                            LocationName = llr.Location != null ? llr.Location.Name : "",
                            CommodityName = lr.Line != null && lr.Line.Commodity != null ? lr.Line.Commodity.Name : "",
                            LdtIssueDate = llr.IssuedOn,
                            LineListStatusIsHardRevision = llr.LineListStatus != null && llr.LineListStatus.IsHardRevision,
                            LineListStatusIsIssuedOfId = llr.LineListStatus != null && llr.LineListStatus.IsIssuedOfId.HasValue
                        };

            return query.OrderBy(l => l.LineNumber).ToList();
        }
        [HttpPost]
        [Authorize(Roles = "CenovusAdmin")]
        public async Task<JsonResult> ToggleActiveStatus([FromBody] List<ActiveSetting> activeSettings)
        {
            try
            {
                if (activeSettings == null || !activeSettings.Any())
                {
                    return Json(new { success = false, error = "No changes provided." });
                }

                foreach (var setting in activeSettings)
                {
                    var lineRevision = await _lineRevisionService.GetById(setting.LineRevisionId);
                    if (lineRevision == null)
                    {
                        return Json(new { success = false, error = $"Line Revision {setting.LineRevisionId} not found." });
                    }

                    lineRevision.IsActive = setting.IsActive;
                    await _lineRevisionService.Update(lineRevision);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = $"Error saving changes: {ex.Message}" });
            }
        }
        //[HttpGet]
        //public async Task<IActionResult> EditOperatingMode(Guid id)
        //{
        //    var mode = await _lineRevisionOperatingModeService.GetById(id);
        //    if (mode == null) return NotFound();
        //    var vm = _mapper.Map<OperatingModeEditViewModel>(mode);
        //    return PartialView("_EditOperatingMode", vm);
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteOperatingMode(Guid id)
        //{
        //    await _lineRevisionOperatingModeService.Remove(id);
        //    return Json(new { success = true });
        //}



    }
}
