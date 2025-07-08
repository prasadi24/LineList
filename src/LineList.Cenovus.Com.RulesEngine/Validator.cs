// Part 1 of 6 — Header, fields, constructor injection

using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using System.Collections.Concurrent;
using System.Reflection;
//using LineList.Cenovus.Com.RulesEngine.Extensions;

namespace LineList.Cenovus.Com.RulesEngine
{
    public class Validator
    {
        // ──────────── SERVICE DEPENDENCIES ────────────
        private readonly IAreaService _areaService;
        private readonly ICodeService _codeService;
        private readonly ICommodityService _commodityService;
        private readonly ICorrosionAllowanceService _corrosionAllowanceService;
        private readonly ICsaClassLocationService _csaClassLocationService;
        private readonly ICsaHvpLvpService _csaHvpLvpService;
        private readonly IEpCompanyService _epCompanyService;
        private readonly IEpProjectService _epProjectService;
        private readonly ILineListRevisionService _lineListRevisionService;
        private readonly IFluidService _fluidService;
        private readonly IFluidPhaseService _fluidPhaseService;
        private readonly IInsulationMaterialService _insulationMaterialService;
        private readonly IInsulationThicknessService _insulationThicknessService;
        private readonly IInsulationTypeService _insulationTypeService;
        private readonly IInternalCoatingLinerService _internalCoatingLinerService;
        private readonly ILineStatusService _lineStatusService;
        private readonly ILineListModelService _lineListService;
        private readonly ILocationService _locationService;
        private readonly INdeCategoryService _ndeCategoryService;
        private readonly IOperatingModeService _operatingModeService;
        private readonly IPaintSystemService _paintSystemService;
        private readonly IPipeSpecificationService _pipeSpecificationService;
        private readonly IPostWeldHeatTreatmentService _postWeldHeatTreatmentService;
        private readonly IPressureProtectionService _pressureProtectionService;
        private readonly IScheduleService _scheduleService;
        private readonly ISizeNpsService _sizeNpsService;
        private readonly ISpecificationService _specificationService;
        private readonly IStressAnalysisService _stressAnalysisService;
        private readonly ITestMediumService _testMediaService;
        private readonly ITracingDesignNumberOfTracersService _tracingDesignNumberOfTracersService;
        private readonly ITracingTypeService _tracingTypeService;
        private readonly IXrayService _xrayService;
        private readonly IValidationService _validationService;
        private readonly IValidationRuleService _validationRuleService;
        private readonly IValidationFieldService _validationFieldService;
        private readonly ILineRevisionService _lineRevisionService;
        private readonly ILineRevisionSegmentService _lineRevisionSegmentService;
        private readonly FlatFactory _flatFactory;
        // tokens & rules
        private readonly string[] opToken = { "1", string.Empty, null };
        private readonly int?[] segmentCountRuleNumber = { 85, 69, 70, 63, 64, 60, 57 };
       
        private IEnumerable<ValidationRule> ValidationRules;
        private IEnumerable<Validation> Validations;
        private string[] CsaCodes;

        // Lookup caches
        private string[] Areas;
        private string[] Codes;
        private string[] Commodities;
        private string[] CorrosionAllowances;
        private string[] CsaClassLocations;
        private string[] CsaHvpLvps;
        private string[] EpCompanies;
        private string[] EpProjects;
        private string[] EpProjectCompanies;
        private string[] EpCompanyProjects;
        private string[] Fluids;
        private string[] FluidPhases;
        private string[] InsulationMaterials;
        private string[] InsulationThicknesses;
        private string[] InsulationTypes;
        private string[] InternalCoatingLiners;
        private string[] LineStatuses;
        private string[] LineListRevisions;
        private string[] LineListRevisionStatus;
        private string[] Locations;
        private string[] NdeCategories;
        private string[] OperatingModes;
        private string[] PaintSystems;
        private string[] PipeSpecifications;
        private string[] PostWeldHeatTreatments;
        private string[] PressureProtections;
        private string[] Schedules;
        private string[] SizeNps_s;
        private string[] Specifications;
        private string[] StressAnalysises;
        private string[] TestMedia;
        private string[] TracingDesignNumberOfTracers;
        private string[] TracingTypes;
        private string[] Xrays;

        // Hard revision required fields
        private string[] HardRevisionFieldsPipe;
        private string[] HardRevisionFieldsAnnulus;
        private string[] HardRevisionFieldsAltOp;
        private string[] HardRevisionFieldsAltOpBlank;
        private string[] HardRevisionFieldsSegments;
        private string[] CsaFields;

        private bool IsCached;
        private bool IncludeHardRevisionRequirements;
        public string ImportFacilityName { get; set; }

        public Validator(
            IAreaService areaService,
            ICodeService codeService,
            ICommodityService commodityService,
            ICorrosionAllowanceService corrosionAllowanceService,
            ICsaClassLocationService csaClassLocationService,
            ICsaHvpLvpService csaHvpLvpService,
            IEpCompanyService epCompanyService,
            IEpProjectService epProjectService,
            ILineListRevisionService lineListRevisionService,
            IFluidService fluidService,
            IFluidPhaseService fluidPhaseService,
            IInsulationMaterialService insulationMaterialService,
            IInsulationThicknessService insulationThicknessService,
            IInsulationTypeService insulationTypeService,
            IInternalCoatingLinerService internalCoatingLinerService,
            ILineStatusService lineStatusService,
            ILineListModelService lineListService,
            ILocationService locationService,
            INdeCategoryService ndeCategoryService,
            IOperatingModeService operatingModeService,
            IPaintSystemService paintSystemService,
            IPipeSpecificationService pipeSpecificationService,
            IPostWeldHeatTreatmentService postWeldHeatTreatmentService,
            IPressureProtectionService pressureProtectionService,
            IScheduleService scheduleService,
            ISizeNpsService sizeNpsService,
            ISpecificationService specificationService,
            IStressAnalysisService stressAnalysisService,
            ITestMediumService testMediaService,
            ITracingDesignNumberOfTracersService tracingDesignNumberOfTracersService,
            ITracingTypeService tracingTypeService,
            IXrayService xrayService,
            IValidationService validationService,
            IValidationRuleService validationRuleService,
            IValidationFieldService validationFieldService,
            ILineRevisionService lineRevisionService,
            ILineRevisionSegmentService lineRevisionSegmentService,
            FlatFactory flatFactory
        )
        {
            _areaService = areaService;
            _codeService = codeService;
            _commodityService = commodityService;
            _corrosionAllowanceService = corrosionAllowanceService;
            _csaClassLocationService = csaClassLocationService;
            _csaHvpLvpService = csaHvpLvpService;
            _epCompanyService = epCompanyService;
            _epProjectService = epProjectService;
            _lineListRevisionService = lineListRevisionService;
            _fluidService = fluidService;
            _fluidPhaseService = fluidPhaseService;
            _insulationMaterialService = insulationMaterialService;
            _insulationThicknessService = insulationThicknessService;
            _insulationTypeService = insulationTypeService;
            _internalCoatingLinerService = internalCoatingLinerService;
            _lineStatusService = lineStatusService;
            _lineListService = lineListService;
            _locationService = locationService;
            _ndeCategoryService = ndeCategoryService;
            _operatingModeService = operatingModeService;
            _paintSystemService = paintSystemService;
            _pipeSpecificationService = pipeSpecificationService;
            _postWeldHeatTreatmentService = postWeldHeatTreatmentService;
            _pressureProtectionService = pressureProtectionService;
            _scheduleService = scheduleService;
            _sizeNpsService = sizeNpsService;
            _specificationService = specificationService;
            _stressAnalysisService = stressAnalysisService;
            _testMediaService = testMediaService;
            _tracingDesignNumberOfTracersService = tracingDesignNumberOfTracersService;
            _tracingTypeService = tracingTypeService;
            _xrayService = xrayService;
            _validationService = validationService;
            _validationRuleService = validationRuleService;
            _validationFieldService = validationFieldService;
            _lineRevisionService = lineRevisionService;
            _lineRevisionSegmentService = lineRevisionSegmentService;
            _flatFactory = flatFactory;
        }

        // ──────────── END PART 1 ────────────
        // Part 2 of 6 — CacheTables() (up through CsaHvpLvps)

        private void CacheTables()
        {
            if (IsCached) return;

            // delimiter and newline constants
            var delim = FormatConstants.Delimiter;
            var nl = FormatConstants.NewLine;

            // load all validation definitions
            var ValidationRules = _validationRuleService.GetAll().Result;
            var Validations = _validationService.GetAll().Result;

            // lookup arrays
            Areas = _areaService.GetAll().Result
                .Select(m => (m.Name + delim + m.Location.Name + delim + m.Specification.Name).ToUpper())
                .OrderBy(x => x)
                .ToArray();

            CsaCodes = _codeService.GetAll().Result
                .Where(m => m.IsCsa)
                .Select(m => m.Name.ToUpper())
                .ToArray();

            Codes = _codeService.GetAll().Result
                .Select(m => m.Name.ToUpper())
                .ToArray();

            Commodities = _commodityService.GetAll().Result
                .Select(m => (m.Name + delim + m.Specification.Name).ToUpper())
                .ToArray();

            CorrosionAllowances = _corrosionAllowanceService.GetAll().Result
                .Select(m => m.Name.ToUpper())
                .ToArray();

            CsaClassLocations = _csaClassLocationService.GetAll().Result
                .Select(m => m.Name.ToUpper())
                .ToArray();

            CsaHvpLvps = _csaHvpLvpService.GetAll().Result
                .Select(m => m.Name.ToUpper())
                .ToArray();

            // ──────────── continue in Part 3 ────────────

            // Part 3 of 6 — CacheTables() (EpProjects through InternalCoatingLiners)

            EpProjects = _epProjectService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            EpCompanies = _epCompanyService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            EpProjectCompanies = _epProjectService.GetAll().Result
                .Select(m => (m.Name + delim + m.EpCompany.Name).ToUpper())
            .Union(
                _lineListRevisionService.GetAll().Result
                    .Select(m => (m.EpProject.Name + delim + m.EpCompany.Name).ToUpper())
            )
            .Distinct()
            .ToArray();

            EpCompanyProjects = _epProjectService.GetAll().Result
                .Select(m => (m.EpCompany.Name + delim + m.Name).ToUpper())
            .Union(
                _lineListRevisionService.GetAll().Result
                    .Select(m => (m.EpCompany.Name + delim + m.EpProject.Name).ToUpper())
            )
            .Distinct()
            .ToArray();

            Fluids = _fluidService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            FluidPhases = _fluidPhaseService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            InsulationMaterials = _insulationMaterialService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            InsulationThicknesses = _insulationThicknessService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            InsulationTypes = _insulationTypeService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            InternalCoatingLiners = _internalCoatingLinerService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            // ──────────── continue in Part 4 ────────────
            // Part 4 of 6 — CacheTables() (LineStatuses through Xrays and hard‐revision fields)

            LineStatuses = _lineStatusService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            LineListRevisions = _lineListService.GetAll().Result
                .Select(m => m.DocumentNumber.ToUpper().Trim())
            .Distinct()
            .ToArray();

            LineListRevisionStatus = _lineListRevisionService.GetAll().Result
                .OrderBy(m => m.LineList.DocumentNumber)
            .ThenBy(m => m.LineListStatus.Name)
            .Select(m => (m.LineList.DocumentNumber + ":" + m.LineListStatus.Name).ToUpper())
            .ToArray();

            Locations = _locationService.GetAll().Result
                .Select(m => (m.Name + delim + m.Facility.Name).ToUpper())
            .ToArray();

            NdeCategories = _ndeCategoryService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            OperatingModes = _operatingModeService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            PaintSystems = _paintSystemService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            PipeSpecifications = _pipeSpecificationService.GetAll().Result
                .Select(m => (m.Name + delim + m.Specification.Name).ToUpper())
            .ToArray();

            PostWeldHeatTreatments = _postWeldHeatTreatmentService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            PressureProtections = _pressureProtectionService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            Schedules = _scheduleService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            SizeNps_s = _sizeNpsService.GetAll().Result
                .Select(m => m.DecimalValue.ToString())
            .ToArray();

            Specifications = _specificationService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            StressAnalysises = _stressAnalysisService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            TestMedia = _testMediaService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            TracingDesignNumberOfTracers = _tracingDesignNumberOfTracersService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            TracingTypes = _tracingTypeService.GetAll().Result
                .Select(m => (m.Name + delim + m.Specification.Name).ToUpper())
            .ToArray();

            Xrays = _xrayService.GetAll().Result
                .Select(m => m.Name.ToUpper())
            .ToArray();

            // Hard‐revision required fields
            var fields = _validationFieldService.GetAll().Result;
            HardRevisionFieldsPipe = fields
                .Where(m => m.FieldType == (int)ValidationFieldTypeEnum.Pipe)
            .Select(m => m.FieldName)
            .ToArray();

            HardRevisionFieldsAnnulus = fields
                .Where(m => m.FieldType == (int)ValidationFieldTypeEnum.Annulus)
            .Select(m => m.FieldName)
            .ToArray();

            HardRevisionFieldsAltOp = fields
                .Where(m => m.FieldType == (int)ValidationFieldTypeEnum.AltOp)
            .Select(m => m.FieldName)
            .ToArray();

            HardRevisionFieldsAltOpBlank = fields
                .Where(m => m.FieldType == (int)ValidationFieldTypeEnum.AltOpBlank)
            .Select(m => m.FieldName)
            .ToArray();

            HardRevisionFieldsSegments = fields
                .Where(m => m.FieldType == (int)ValidationFieldTypeEnum.Segment)
            .Select(m => m.FieldName)
            .ToArray();

            CsaFields = fields
                .Where(m => m.FieldType == (int)ValidationFieldTypeEnum.Csa)
            .Select(m => m.FieldName)
            .ToArray();

            IsCached = true;
        }
        // Part 5 of 6 — Validation methods (CheckExisting through ValidateSegments)

        /// <summary>
        /// Checks for any incoming flat lines that already exist in the database.
        /// </summary>
        public IList<ValidationException> CheckExisting(IList<FlatLine> lines)
        {
            var exceptions = new List<ValidationException>();
            // reuse a single Validation definition for “already exists”
            var validation = new Validation
            {
                FieldName = "ChildNumber",
                Id = Guid.NewGuid(),
                Message = "Line already exists in database",
                ValidationType = (int)ValidationTypeEnum.Line,
                RuleNumber = 0
            };

            // pull all existing LineRevisions once
            var existingRevs = _lineRevisionService.GetAll().Result;

            foreach (var line in lines)
            {
                if (existingRevs.Any(m =>
                    m.Line.Location.Facility.Name == line.FacilityID &&
                    m.Line.Location.Name == line.Location &&
                    m.Line.Commodity.Name == line.Commodity &&
                    m.Line.SequenceNumber == line.SequenceNumber &&
                    m.Line.ChildNumber == line.ChildNumber))
                {
                    exceptions.Add(new ValidationException(validation, line, null));
                }
            }

            return exceptions;
        }

        /// <summary>
        /// Audits any transitions from active to inactive lookup values on incoming lines.
        /// </summary>
        public IEnumerable<ValidationException> AuditInactiveTransitions(IList<FlatLine> lines, string userName)
        {
            var exceptions = new List<ValidationException>();

            // load all line revisions once
            var allRevs = _lineRevisionService.GetAll().Result;

            // split into jacketed vs non, then create domain LineRevision instances
            var jacketedInput = lines.Where(l => l.IsJacketed).ToList();
            var primaryInput = lines.Where(l => !l.IsJacketed).ToList();            
            var jacketedRevs = _flatFactory.ToLineRevisions(jacketedInput, userName, true, false);
            var nonJacketedRevs = _flatFactory.ToLineRevisions(primaryInput, userName, false, false);
            var allLineRevs = jacketedRevs.Union(nonJacketedRevs).ToList();
            var lineType = (int)ValidationTypeEnum.Line;
            var segmentType = (int)ValidationTypeEnum.Segment;

            // helper to pick first matching validation rule
            Validation PickRule(string field, int type)
                => _validationService.GetAll().Result
                    .Where(v => v.FieldName == field && v.ValidationType == type)
                    .OrderBy(v => v.RuleNumber)
                    .First();

            // for each lookup we care about, check if the new value is inactive while previous was active
            foreach (var rev in allLineRevs)
            {
                // PipeSpecification inactive?
                if (rev.PipeSpecificationId.HasValue &&
                    !_pipeSpecificationService.GetAll().Result.Any(p => p.Id == rev.PipeSpecificationId.Value) &&
                    allRevs.Any(r => r.Id == rev.Id && r.PipeSpecificationId != rev.PipeSpecificationId))
                {
                    exceptions.Add(new ValidationException(
                        PickRule("ClassServMaterial", lineType),
                        lines.First(l => l.Id == rev.Id && opToken.Contains(l.AltOpMode)),
                        null));
                }
                // SizeNpsPipe inactive?
                if (rev.SizeNpsPipeId.HasValue &&
                    !_sizeNpsService.GetAll().Result.Any(s => s.Id == rev.SizeNpsPipeId.Value) &&
                    allRevs.Any(r => r.Id == rev.Id && r.SizeNpsPipeId != rev.SizeNpsPipeId))
                {
                    exceptions.Add(new ValidationException(
                        PickRule("SizeNps", lineType),
                        lines.First(l => l.Id == rev.Id && opToken.Contains(l.AltOpMode)),
                        null));
                }
                // SchedulePipe inactive?
                if (rev.SchedulePipeId.HasValue &&
                    !_scheduleService.GetAll().Result.Any(s => s.Id == rev.SchedulePipeId.Value) &&
                    allRevs.Any(r => r.Id == rev.Id && r.SchedulePipeId != rev.SchedulePipeId))
                {
                    exceptions.Add(new ValidationException(
                        PickRule("Schedule", lineType),
                        lines.First(l => l.Id == rev.Id && opToken.Contains(l.AltOpMode)),
                        null));
                }
                // TestMediumPipe inactive?
                if (rev.TestMediumPipeId.HasValue &&
                    !_testMediaService.GetAll().Result.Any(t => t.Id == rev.TestMediumPipeId.Value) &&
                    allRevs.Any(r => r.Id == rev.Id && r.TestMediumPipeId != rev.TestMediumPipeId))
                {
                    exceptions.Add(new ValidationException(
                        PickRule("TestMedium", lineType),
                        lines.First(l => l.Id == rev.Id && opToken.Contains(l.AltOpMode)),
                        null));
                }
                // CorrosionAllowancePipe inactive?
                if (rev.CorrosionAllowancePipeId.HasValue &&
                    !_corrosionAllowanceService.GetAll().Result.Any(c => c.Id == rev.CorrosionAllowancePipeId.Value) &&
                    allRevs.Any(r => r.Id == rev.Id && r.CorrosionAllowancePipeId != rev.CorrosionAllowancePipeId))
                {
                    exceptions.Add(new ValidationException(
                        PickRule("CorrosionAllowance", lineType),
                        lines.First(l => l.Id == rev.Id && opToken.Contains(l.AltOpMode)),
                        null));
                }
                // XrayPipe inactive?
                if (rev.XrayPipeId.HasValue &&
                    !_xrayService.GetAll().Result.Any(x => x.Id == rev.XrayPipeId.Value) &&
                    allRevs.Any(r => r.Id == rev.Id && r.XrayPipeId != rev.XrayPipeId))
                {
                    exceptions.Add(new ValidationException(
                        PickRule("Xray", lineType),
                        lines.First(l => l.Id == rev.Id && opToken.Contains(l.AltOpMode)),
                        null));
                }

                // now segments (example: insulationThickness)
                foreach (var seg in rev.LineRevisionSegments)
                {
                    if (seg.InsulationThicknessId.HasValue &&
                        !_insulationThicknessService.GetAll().Result.Any(i => i.Id == seg.InsulationThicknessId.Value) &&
                        _lineRevisionService.GetAll().Result
                            .Where(s => s.Id == seg.LineRevisionId && s.LineRevisionSegments.Any(ss => ss.InsulationThicknessId != seg.InsulationThicknessId))
                            .Any())
                    {
                        exceptions.Add(new ValidationException(
                            PickRule("InsulationThickness", segmentType),
                            lines.First(l => l.Id == rev.Id && opToken.Contains(l.AltOpMode)),
                            null));
                    }
                }
            }

            // append common trailing message
            const string msg = " Existing lines may use inactive lookup table values, but new values must be active.";
            foreach (var ex in exceptions)
                if (!ex.Validation.Message.Contains(msg))
                    ex.Validation.Message += msg;

            return exceptions;
        }

        /// <summary>
        /// Validates entire line‐list header rows.
        /// </summary>
        public IList<ValidationException> Validate(IList<FlatLineList> lineLists)
        {
            CacheTables();

            var headerRules = _validationService.GetAll().Result
                .Where(v => v.ValidationType == (int)ValidationTypeEnum.LineList)
                .ToList();

            var exceptions = new List<ValidationException>();
            foreach (var rule in headerRules)
            {
                var invalid = GetInvalid(lineLists, rule);
                foreach (var ll in invalid)
                    exceptions.Add(new ValidationException { LineList = ll, Validation = rule });
            }

            return exceptions;
        }

        /// <summary>
        /// Validates all individual flat lines (pipe, alt op, jacket, segments, hard‐revision, etc.).
        /// </summary>
        public IList<ValidationException> Validate(IList<FlatLine> lines)
        {
            CacheTables();

            var exceptions = new List<ValidationException>();

            // ensure jacket flags are correct
            VerifyJacket(lines);

            // split primary vs alt‐op vs jacket
            var primaries = lines.Where(l => opToken.Contains(l.AltOpMode)).ToList();
            var alternates = lines.Where(l => !opToken.Contains(l.AltOpMode)).ToList();
            var jacketed = primaries.Where(l => l.IsJacketed).ToList();

            // top‐level rules
            var lineRules = _validationService.GetAll().Result.Where(v => v.ValidationType == (int)ValidationTypeEnum.Line).ToList();
            var altRules = _validationService.GetAll().Result.Where(v => v.ValidationType == (int)ValidationTypeEnum.AltOpMode).ToList();
            var jackRules = _validationService.GetAll().Result
                .Where(v => v.ValidationType == (int)ValidationTypeEnum.JacketedLine)
                .Union(lineRules)
                .ToList();
            var segRules = _validationService.GetAll().Result.Where(v => v.ValidationType == (int)ValidationTypeEnum.Segment).ToList();

            // run each group
            exceptions.AddRange(ValidateLines(lineRules, primaries));
            exceptions.AddRange(ValidateLines(altRules, alternates));
            exceptions.AddRange(ValidateLines(jackRules, jacketed));
            exceptions.AddRange(ValidateSegments(segRules, lines));

            // hard‐revision requirements if enabled
            if (IncludeHardRevisionRequirements)
            {
                exceptions.AddRange(ValidateForIssuingHardRevision(HardRevisionFieldsPipe, primaries, "", "", false));
                exceptions.AddRange(ValidateForIssuingHardRevision(HardRevisionFieldsAnnulus, jacketed, "", " on a jacketed line.", false));
                exceptions.AddRange(ValidateForIssuingHardRevision(HardRevisionFieldsAltOp, alternates, "", " with an Alternate Operating Mode.", false));

                // aggregate segments for both primary & jacketed
                var allForSegments = primaries.Concat(jacketed).Where(l => l.RequiresValidation).ToList();
                var segs = ToSegments(allForSegments, new List<ValidationException>());
                exceptions.AddRange(ValidateForIssuingHardRevision(HardRevisionFieldsSegments, segs, "", "", true));
            }

            return exceptions;
        }

        /// <summary>
        /// Marks any FlatLine as jacketed if its TraceDesignType contains a jacketed tracer.
        /// </summary>
        public void VerifyJacket(IList<FlatLine> lines)
        {
            // pull names of all jacketed tracing types
            var traceTypes = _tracingTypeService.GetAll().Result
                .Where(t => t.IsJacketed)
                .Select(t => t.Name)
                .ToArray();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line.TraceDesignType))
                    continue;
                foreach (var type in traceTypes)
                {
                    if (line.TraceDesignType.Contains(type))
                    {
                        line.IsJacketed = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Applies a set of validation rules to a homogeneous list of FlatLine rows.
        /// </summary>
        private IList<ValidationException> ValidateLines(
            IList<Validation> rules,
            IList<FlatLine> lines)
        {
            var exceptions = new List<ValidationException>();

            foreach (var rule in rules)
            {
                var invalid = GetInvalid(lines, rule);
                foreach (var line in invalid)
                {
                    if (!exceptions.Any(e =>
                        e.Line.Id == line.Id &&
                        e.Validation.FieldName == rule.FieldName))
                    {
                        exceptions.Add(new ValidationException { Line = line, Validation = rule });
                    }
                }
            }

            return exceptions;
        }

        /// <summary>
        /// Applies segment‐level validation rules by first flattening to per‐segment rows.
        /// </summary>
        private IList<ValidationException> ValidateSegments(
            IList<Validation> rules,
            IList<FlatLine> lines)
        {
           
            string fieldName = string.Empty;


            var exceptions = new List<ValidationException>();

            // build a flat list of “segment” rows
            var segments = ToSegments(lines, exceptions);

            foreach (var rule in rules)
            {
                var invalid = GetInvalid(segments, rule);
                foreach (var seg in invalid)
                {
                    var orig = lines.First(l =>
                        l.Id == seg.Id && opToken.Contains(l.AltOpMode));
                    exceptions.Add(new ValidationException { Line = orig, Validation = rule });
                }
            }

            return exceptions;
        }
        /// <summary>
        /// Returns the subset of 'items' that fail the given Validation.
        /// </summary>
        private List<T> GetInvalid<T>(IList<T> items, Validation rule)
        {
            var invalid = new List<T>();

            // determine which ValidationRule tokens to apply
            var tokens = rule.Rules != null && rule.Rules.Any()
                ? rule.Rules
                : _validationRuleService.GetAll().Result
                    .Where(r => r.ValidationId == rule.Id)
                    .OrderBy(r => r.SortOrder)
                    .ToList();

            foreach (var item in items)
            {
                bool overallValid = true;
                bool lastCheck = true;

                foreach (var token in tokens)
                {
                    switch ((RuleTypeEnum)token.RuleType)
                    {
                        case RuleTypeEnum.FormatCheck:
                            lastCheck = ValidFormat(item, token.FieldName);
                            break;
                        case RuleTypeEnum.Lookup:
                            lastCheck = ValidLookup(item, token.FieldName, token.Value, token.Criteria);
                            break;
                        case RuleTypeEnum.MinLength:
                            lastCheck = ValidMinLength(item, token.FieldName, token.Value);
                            break;
                        case RuleTypeEnum.MaxLength:
                            lastCheck = ValidMaxLength(item, token.FieldName, token.Value);
                            break;
                        case RuleTypeEnum.MustBe:
                            lastCheck = ValidMustBe(item, token.FieldName, token.Value);
                            break;
                        case RuleTypeEnum.TypeCheck:
                            lastCheck = ValidType(item, token.FieldName, token.Value);
                            break;
                        case RuleTypeEnum.NotExist:
                            lastCheck = ValidNotExist(item, token.FieldName, token.Value, items);
                            break;
                        case RuleTypeEnum.Match:
                            lastCheck = ValidMatch(item, items, token.FieldName);
                            break;
                        default:
                            lastCheck = true;
                            break;
                    }

                    if (token.RuleOperator == (int)RuleOperatorEnum.And)
                        overallValid = overallValid && lastCheck;
                    else if (token.RuleOperator == (int)RuleOperatorEnum.Or)
                        overallValid = overallValid || lastCheck;
                    else
                        overallValid = lastCheck;
                }

                if (!overallValid)
                    invalid.Add(item);
            }

            return invalid;
        }

        /// <summary>
        /// Checks whether the named field parses as a DateTime.
        /// </summary>
        private bool ValidFormat(object obj, string fieldName)
        {
            var text = GetFieldValue(obj, fieldName);
            return DateTime.TryParse(text, out _);
        }

        /// <summary>
        /// Checks that the named field’s length ≥ minLength.
        /// </summary>
        private bool ValidMinLength(object obj, string fieldName, string minLength)
        {
            var text = GetFieldValue(obj, fieldName);
            return text?.Length >= int.Parse(minLength);
        }

        /// <summary>
        /// Checks that the named field’s length ≤ maxLength.
        /// </summary>
        private bool ValidMaxLength(object obj, string fieldName, string maxLength)
        {
            var text = GetFieldValue(obj, fieldName);
            return text?.Length <= int.Parse(maxLength);
        }

        /// <summary>
        /// Checks that the named field equals (case-insensitive) the given value.
        /// </summary>
        private bool ValidMustBe(object obj, string fieldName, string required)
        {
            var text = (GetFieldValue(obj, fieldName) ?? string.Empty).Trim().ToLower();
            return text == (required ?? string.Empty).Trim().ToLower();
        }

        /// <summary>
        /// Checks numeric or decimal ranges per token.Value ("Integer;min;max" or "Decimal;min;max;scale").
        /// </summary>
        private bool ValidType(object obj, string fieldName, string typeToken)
        {
            var parts = typeToken.Split(';');
            var text = GetFieldValue(obj, fieldName);

            if (parts[0] == "Integer")
            {
                if (int.TryParse(text, out var iv))
                    return iv >= int.Parse(parts[1]) && iv <= int.Parse(parts[2]) && !text.Contains(".");

                return false;
            }
            else if (parts[0] == "Decimal")
            {
                if (decimal.TryParse(text, out var dv))
                {
                    var min = decimal.Parse(parts[1]);
                    var max = decimal.Parse(parts[2]);
                    if (dv < min || dv > max) return false;
                    if (parts.Length > 3)
                    {
                        var scale = int.Parse(parts[3]);
                        var actualScale = GetDecimalPlaces(text);
                        return actualScale <= scale;
                    }
                    return true;
                }
                return false;
            }

            return true;
        }

        /// <summary>
        /// Ensures the value is not already present (or unique per criteria).
        /// </summary>
        private bool ValidNotExist<T>(T obj, string fieldName, string value, IList<T> list)
        {
            var text = GetFieldValue(obj, fieldName);
            if (string.IsNullOrWhiteSpace(value))
                return text != value;

            if (value == "DocumentNumber")
            {
                return list.Select(x => GetFieldValue(x, fieldName)).Count(x => x == text) == 1;
            }

            return !list.Any(x => GetFieldValue(x, fieldName) == text);
        }

        /// <summary>
        /// Validates that the field matches the same field on an adjacent item.
        /// </summary>
        private bool ValidMatch<T>(T obj, IList<T> list, string fieldName)
        {
            var idx = list.IndexOf(obj);
            var me = GetFieldValue(obj, fieldName);
            bool matchPrev = idx > 0 && GetFieldValue(list[idx - 1], fieldName) == me;
            bool matchNext = idx < list.Count - 1 && GetFieldValue(list[idx + 1], fieldName) == me;
            return matchPrev || matchNext;
        }

        /// <summary>
        /// Helper to count digits after the decimal point.
        /// </summary>
        private int GetDecimalPlaces(string s)
        {
            var idx = s.IndexOf('.');
            return idx < 0 ? 0 : (s.Length - idx - 1);
        }

        /// <summary>
        /// Checks that the named field’s value exists in the specified lookup table.
        /// </summary>
        private bool ValidLookup(object obj, string fieldName, string lookup, string criteria)
        {
            var raw = GetFieldValue(obj, fieldName);
            var value = (raw ?? string.Empty).Trim().ToUpper();

            if (!string.IsNullOrWhiteSpace(criteria))
            {
                foreach (var crit in criteria.Split(';'))
                {
                    var parts = crit.Split(':');
                    if (parts.Length == 2)
                    {
                        var extra = GetFieldValue(obj, parts[0]);
                        value = value + FormatConstants.Delimiter + (extra ?? string.Empty).Trim().ToUpper();
                    }
                }
            }

            switch (lookup)
            {
                case "Area.Name":
                    return _areaService.GetAll().Result.Select(a => a.Name.ToUpper()).Contains(value);
                case "Code.Name":
                    return _codeService.GetAll().Result.Select(c => c.Name.ToUpper()).Contains(value);
                case "Commodity.Name":
                    return _commodityService.GetAll().Result
                        .Select(c => (c.Name + FormatConstants.Delimiter + c.Specification.Name).ToUpper())
                        .Contains(value);
                case "CorrosionAllowance.Name":
                    return _corrosionAllowanceService.GetAll().Result.Select(ca => ca.Name.ToUpper()).Contains(value);
                case "CsaClassLocation.Name":
                    return _csaClassLocationService.GetAll().Result.Select(csl => csl.Name.ToUpper()).Contains(value);
                // … add other lookup cases here, always calling .Result on GetAll() …
                default:
                    return true;
            }
        }

        /// <summary>
        /// Retrieves the string value of the given public property or field.
        /// </summary>
        private string GetFieldValue(object obj, string name)
        {
            if (obj == null || string.IsNullOrEmpty(name))
                return string.Empty;

            var type = obj.GetType();
            var prop = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (prop != null)
                return prop.GetValue(obj)?.ToString().Trim() ?? string.Empty;

            var field = type.GetField(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            if (field != null)
                return field.GetValue(obj)?.ToString().Trim() ?? string.Empty;

            return string.Empty;
        }

        private IList<ValidationException> ValidateForIssuingHardRevision(string[] requiredFields, IList<FlatLine> lines, string prefix, string suffix, bool isSegment)
        {
            ConcurrentBag<ValidationException> exceptions = new ConcurrentBag<ValidationException>();

#if DEBUG //when you're debugging, avoid the parrelel threads
            foreach (var field in requiredFields)
#else

            Parallel.ForEach(requiredFields, field =>
#endif
            {
                var rule = new Validation()
                {
                    FieldName = field.Replace("pipe", "").Replace("annu", ""),
                    RuleNumber = 0,
                    ValidationType = (int)ValidationTypeEnum.Line,
                    Id = Guid.NewGuid(),
                    Message = field.Replace("pipe", prefix).Replace("annu", prefix) + " is required in order to issue a hard revision" + suffix
                };
                var validationRule = new ValidationRule()
                {
                    FieldName = field,
                    RuleOperator = (int)RuleOperatorEnum.None,
                    RuleType = (int)RuleTypeEnum.MinLength,
                    Value = "1"
                };
                rule.Rules = new List<ValidationRule>() { validationRule };

                var invalid = GetInvalid<FlatLine>(lines.Where(m => m.RequiresValidation == true).ToList(), rule);
                invalid.ForEach(m => exceptions.Add(new ValidationException() { Line = lines.Where(z => z.Id == m.Id && z.AltOpMode == m.AltOpMode).First(), Validation = rule }));
#if DEBUG
            }
#else
            });
#endif
            if (!isSegment)
            {
                //make sure the required csa fields are filled in
                foreach (var field in CsaFields)
                {
                    var rule = new Validation()
                    {
                        FieldName = field,
                        RuleNumber = 0,
                        ValidationType = (int)ValidationTypeEnum.Line,
                        Id = Guid.NewGuid(),
                        Message = field + " is required in order to issue a hard revision when a CSA code is selected" + suffix
                    };
                    var validationRule = new ValidationRule()
                    {
                        FieldName = field,
                        RuleOperator = (int)RuleOperatorEnum.None,
                        RuleType = (int)RuleTypeEnum.MinLength,
                        Value = "1"
                    };
                    rule.Rules = new List<ValidationRule>() { validationRule };

                    var invalid = GetInvalid<FlatLine>(lines.Where(m => m.RequiresValidation == true && CsaCodes.Contains(m.Code)).ToList(), rule);
                    invalid.ForEach(m => exceptions.Add(new ValidationException() { Line = lines.Single(z => z.Id == m.Id && z.AltOpMode == m.AltOpMode), Validation = rule }));
                }

                //make sure the CSA fields are blank when not required
                foreach (var field in CsaFields)
                {
                    var rule = new Validation()
                    {
                        FieldName = field,
                        RuleNumber = 99,
                        ValidationType = (int)ValidationTypeEnum.Line,
                        Id = Guid.NewGuid(),
                        Message = field + " must be blank in order to issue a hard revision when a CSA code is not selected" + suffix
                    };
                    var validationRule = new ValidationRule()
                    {
                        FieldName = field,
                        RuleOperator = (int)RuleOperatorEnum.None,
                        RuleType = (int)RuleTypeEnum.MustBe,
                        Value = string.Empty
                    };
                    rule.Rules = new List<ValidationRule>() { validationRule };

                    var invalid = GetInvalid<FlatLine>(lines.Where(m => !CsaCodes.Contains(m.Code)).ToList(), rule);
                    invalid.ForEach(m => exceptions.Add(new ValidationException() { Line = lines.Single(z => z.Id == m.Id && z.AltOpMode == m.AltOpMode), Validation = rule }));
                }

                //make sure Schedule or Wall thickness is filled out, but not both
                foreach (var field in new string[] { "Schedule", "WallThickness" })
                {
                    var rule = new Validation()
                    {
                        FieldName = field,
                        RuleNumber = 0,
                        ValidationType = (int)ValidationTypeEnum.Line,
                        Id = Guid.NewGuid(),
                        Message = "Either Schedule or Wall Thickness (but not both) is required in order to issue a hard revision" + suffix
                    };
                    var validationRule = new ValidationRule()
                    {
                        FieldName = field,
                        RuleOperator = (int)RuleOperatorEnum.None,
                        RuleType = (int)RuleTypeEnum.MinLength,
                        Value = "1"
                    };
                    rule.Rules = new List<ValidationRule>() { validationRule };

                    var invalid = GetInvalid<FlatLine>(lines.Where(m => m.RequiresValidation == true && string.IsNullOrWhiteSpace(m.Schedule) && string.IsNullOrWhiteSpace(m.WallThickness) && opToken.Contains(m.AltOpMode)).ToList(), rule);
                    invalid.ForEach(m => exceptions.Add(new ValidationException() { Line = lines.Where(z => z.Id == m.Id).First(), Validation = rule }));
                }

                foreach (var item in lines)
                {
                    //add an error if they're both filled out.
                    if ((!string.IsNullOrWhiteSpace(item.pipeSchedule) && !string.IsNullOrWhiteSpace(item.pipeWallThickness)) || (!string.IsNullOrWhiteSpace(item.annuSchedule) && !string.IsNullOrWhiteSpace(item.annuWallThickness)))
                    {
                        exceptions.Add(new ValidationException() { Line = item, Validation = new Validation() { Id = Guid.NewGuid(), FieldName = "Schedule", ValidationType = (int)ValidationTypeEnum.Line, Message = "Either Schedule or Wall Thickness can be filled in, but not both" + suffix, RuleNumber = -1 } });
                        exceptions.Add(new ValidationException() { Line = item, Validation = new Validation() { Id = Guid.NewGuid(), FieldName = "WallThickness", ValidationType = (int)ValidationTypeEnum.Line, Message = "Either Schedule or Wall Thickness can be filled in, but not both" + suffix, RuleNumber = -1 } });
                    }
                }
            }
            return exceptions.ToList();
        }
        private IList<FlatLine> ToSegments(IList<FlatLine> lines, IList<ValidationException> exception)
        {
            var segments = new List<FlatLine>();
            //using (var db = new LLDatabase())
            // {
            var validations =  _validationService.GetAll().Result;
                var segmentCountValidationRules = validations.Where(m => segmentCountRuleNumber.Contains(m.RuleNumber));
                string[] seperator = new string[] { FormatConstants.NewLine };

                if (lines.Count == 0)
                    return segments;

                foreach (var line in lines)
                {
                    if (ValidSegmentCount(line, seperator))
                    {
                        var count = line.InsulationType.Split(seperator, StringSplitOptions.None).Length;
                        if (line.InsulationType.LastIndexOf('\n') == line.InsulationType.Length - 1)
                            count--; //disregard the last part, since it's empty
                        count = (count == 0) ? 1 : count;
                        for (int i = 0; i < count; i++)
                        {
                            segments.Add(ToSegment(line, i, seperator));
                        }
                    }
                    else
                    {
                        foreach (var segmentCountValidationRule in segmentCountValidationRules)
                            exception.Add(new ValidationException() { Line = line, Validation = segmentCountValidationRule });
                    }
                }
           // }
            return segments;
        }

        private FlatLine ToSegment(FlatLine line, int index, string[] seperator)
        {
            line.InsulationMaterial = line.InsulationMaterial ?? string.Empty;
            line.InsulationThickness = line.InsulationThickness ?? string.Empty;
            line.InsulationType = line.InsulationType ?? string.Empty;
            line.PaintSystem = line.PaintSystem ?? string.Empty;
            line.TraceDesignType = line.TraceDesignType ?? string.Empty;
            line.TracingDesignNumTracers = line.TracingDesignNumTracers ?? string.Empty;
            line.TraceDesignHoldTemp = line.TraceDesignHoldTemp ?? string.Empty;

            var insulationMaterial = line.InsulationMaterial.Contains(seperator[0].ToString()) ? GetValue(line.InsulationMaterial.Split(seperator, StringSplitOptions.None)[index], ':') : GetValue(line.InsulationMaterial, ':');
            var insulationThickness = line.InsulationThickness.Contains(seperator[0].ToString()) ? GetValue(line.InsulationThickness.Split(seperator, StringSplitOptions.None)[index], ':') : GetValue(line.InsulationThickness, ':');
            var insulationType = line.InsulationType.Contains(seperator[0].ToString()) ? GetValue(line.InsulationType.Split(seperator, StringSplitOptions.None)[index], ':') : GetValue(line.InsulationType, ':');
            var paintSystem = line.PaintSystem.Contains(seperator[0].ToString()) ? GetValue(line.PaintSystem.Split(seperator, StringSplitOptions.None)[index], ':') : GetValue(line.PaintSystem, ':');
            var traceDesignHoldTemp = line.TraceDesignHoldTemp.Contains(seperator[0].ToString()) ? GetValue(line.TraceDesignHoldTemp.Split(seperator, StringSplitOptions.None)[index], ':') : GetValue(line.TraceDesignHoldTemp, ':');
            var traceDesignType = line.TraceDesignType.Contains(seperator[0].ToString()) ? GetValue(line.TraceDesignType.Split(seperator, StringSplitOptions.None)[index], ':') : GetValue(line.TraceDesignType, ':');
            var tracingDesignNumTracers = line.TracingDesignNumTracers.Contains(seperator[0].ToString()) ? GetValue(line.TracingDesignNumTracers.Split(seperator, StringSplitOptions.None)[index], ':') : GetValue(line.TracingDesignNumTracers, ':');
            var segmentType = line.InsulationType.Contains(seperator[0].ToString()) ? GetValue(line.InsulationType.Split(seperator, StringSplitOptions.None)[index], ':', 0) : GetValue(line.InsulationType, ':', 0);
            var segmentNumber = (index + 1).ToString();

            var seg = new FlatLine()
            {
                Id = line.Id,
                SegmentType = segmentType,
                SegmentNumber = segmentNumber,
                InsulationMaterial = insulationMaterial,
                InsulationThickness = insulationThickness,
                InsulationType = insulationType,
                PaintSystem = paintSystem,
                TraceDesignHoldTemp = traceDesignHoldTemp,
                TraceDesignType = traceDesignType,
                TracingDesignNumTracers = tracingDesignNumTracers,
                Specification = line.Specification,
                RequiresValidation = line.RequiresValidation,
                AltOpMode = line.AltOpMode,
                Source = line.Source
            };
            return seg;
        }

        private string GetValue(string text, char seperator)
        {
            return GetValue(text, seperator, 1);
        }

        private string GetValue(string text, char seperator, int index)
        {
            if (text.Contains(seperator))
                return text.Split(seperator)[index].Trim();
            else
                return string.IsNullOrWhiteSpace(text) ? string.Empty : text.Trim();
        }

        internal bool ValidSegmentCount(FlatLine line, string[] seperator)
        {
            bool valid = false;

            line.InsulationType = line.InsulationType ?? string.Empty;
            line.InsulationThickness = line.InsulationThickness ?? string.Empty;
            line.PaintSystem = line.PaintSystem ?? string.Empty;
            line.TraceDesignType = line.TraceDesignType ?? string.Empty;
            line.TracingDesignNumTracers = line.TracingDesignNumTracers ?? string.Empty;
            line.TraceDesignHoldTemp = line.TraceDesignHoldTemp ?? string.Empty;
            line.InsulationMaterial = line.InsulationMaterial ?? string.Empty;

            var insulationTypeLength = line.InsulationType.Split(seperator, StringSplitOptions.None).Length;
            var insulationMaterialLength = line.InsulationMaterial.Split(seperator, StringSplitOptions.None).Length;
            var insulationThicknessLength = line.InsulationThickness.Split(seperator, StringSplitOptions.None).Length;
            var paintSystemLength = line.PaintSystem.Split(seperator, StringSplitOptions.None).Length;
            var traceDesignHoldTempLength = line.TraceDesignHoldTemp.Split(seperator, StringSplitOptions.None).Length;
            var traceDesignTypeLength = line.TraceDesignType.Split(seperator, StringSplitOptions.None).Length;
            var traceDesignNumTracerLength = line.TracingDesignNumTracers.Split(seperator, StringSplitOptions.None).Length;

            //if they're zero that means it's actually one
            insulationTypeLength = insulationTypeLength == 0 ? 1 : insulationTypeLength;
            insulationThicknessLength = insulationThicknessLength == 0 ? 1 : insulationThicknessLength;
            paintSystemLength = paintSystemLength == 0 ? 1 : paintSystemLength;
            traceDesignHoldTempLength = traceDesignHoldTempLength == 0 ? 1 : traceDesignHoldTempLength;
            traceDesignTypeLength = traceDesignTypeLength == 0 ? 1 : traceDesignTypeLength;
            traceDesignNumTracerLength = traceDesignNumTracerLength == 0 ? 1 : traceDesignNumTracerLength;
            insulationMaterialLength = insulationMaterialLength == 0 ? 1 : insulationMaterialLength;

            if (insulationTypeLength == insulationThicknessLength)
                if (insulationThicknessLength == paintSystemLength)
                    if (paintSystemLength == traceDesignHoldTempLength)
                        if (traceDesignHoldTempLength == traceDesignTypeLength)
                            if (traceDesignTypeLength == traceDesignNumTracerLength)
                                if ((line.Source == FlatSourceEnum.Revision && traceDesignNumTracerLength == insulationMaterialLength) || line.Source == FlatSourceEnum.Import)
                                    valid = SegmentTypesMatch(line);
            return valid;
        }

        private bool SegmentTypesMatch(FlatLine line)
        {
            var delimiter = FormatConstants.NewLine;

            var list = new List<string>();

            list.Add(GetSegmentList(line.InsulationThickness, delimiter));
            list.Add(GetSegmentList(line.InsulationType, delimiter));
            list.Add(GetSegmentList(line.PaintSystem, delimiter));
            list.Add(GetSegmentList(line.TraceDesignHoldTemp, delimiter));
            list.Add(GetSegmentList(line.TraceDesignType, delimiter));
            list.Add(GetSegmentList(line.TracingDesignNumTracers, delimiter));

            if (line.Source != FlatSourceEnum.Import) //Insulation Material isn't validated on import
                list.Add(GetSegmentList(line.InsulationMaterial, delimiter));

            //if there is only 1 segment, then the segment type can be blank
            var count = line.InsulationMaterial.Split(new string[] { delimiter }, StringSplitOptions.None).Length;
            bool returnValue = false;
            if (count == 1)
                returnValue = list.Where(m => m.Trim() != "").Distinct().Count() < 2;
            else
                returnValue = list.Distinct().Count() < 2;

            return returnValue;
        }

        private string GetSegmentList(string input, string delimiter)
        {
            var values = string.Empty;
            var sectionDelimiter = ':';

            if (input.Contains(delimiter) || input.Contains(sectionDelimiter))
                foreach (var item in input.Split(new string[] { delimiter }, StringSplitOptions.None))
                    values += item.Split(sectionDelimiter)[0].Trim();

            return values;
        }
    }
}
