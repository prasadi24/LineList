using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using System.Collections.Concurrent;
using System.Text;

namespace LineList.Cenovus.Com.RulesEngine
{
    public partial class FlatFactory
    {
        // ───────────────────────────────────────────────
        // injected services
        private readonly ILineRevisionService _lineRevisionService;
        private readonly ITracingTypeService _tracingTypeService;
        private readonly ILineService _lineService;
        private readonly IAreaService _areaService;
        private readonly ICodeService _codeService;
        private readonly ICommodityService _commodityService;
        private readonly ICorrosionAllowanceService _corrosionAllowanceService;
        private readonly ICsaClassLocationService _csaClassLocationService;
        private readonly ICsaHvpLvpService _csaHvpLvpService;
        private readonly IEpProjectService _epProjectService;
        private readonly IEpCompanyService _epCompanyService;
        private readonly ILineListRevisionService _lineListRevisionService;
        private readonly ISegmentTypeService _segmentTypeService;
        private readonly IInsulationThicknessService _insulationThicknessService;
        private readonly IInsulationTypeService _insulationTypeService;
        private readonly ITracingDesignNumberOfTracersService _tracingDesignNumberOfTracersService;
        private readonly IInsulationMaterialService _insulationMaterialService;
        private readonly IPaintSystemService _paintSystemService;
        private readonly IOperatingModeService _operatingModeService;
        private readonly IFluidService _fluidService;
        private readonly IFluidPhaseService _fluidPhaseService;
        private readonly IPressureProtectionService _pressureProtectionService;
        private readonly IPipeSpecificationService _pipeSpecificationService;
        private readonly IPostWeldHeatTreatmentService _postWeldHeatTreatmentService;
        private readonly IStressAnalysisService _stressAnalysisService;
        private readonly IInternalCoatingLinerService _internalCoatingLinerService;
        private readonly ITestMediumService _testMediumService;
        private readonly IXrayService _xrayService;
        private readonly INdeCategoryService _ndeCategoryService;
        private readonly IScheduleService _scheduleService;
        private readonly ISizeNpsService _sizeNpsService;
        private readonly ILineStatusService _lineStatusService;
        private readonly ILineListStatusService _lineListStatusService;
        private readonly ILocationService _locationService;
        // ───────────────────────────────────────────────

        private Dictionary<string, Guid> _lineListRevisionIds;
        private static List<string> unnumberedChildren = new List<string>();

        private static readonly string[] opTokens = { "1", string.Empty, null };

        public FlatFactory(
            ILineRevisionService lineRevisionService,
            ITracingTypeService tracingTypeService,
            ILineService lineService,
            IAreaService areaService,
            ICodeService codeService,
            ICommodityService commodityService,
            ICorrosionAllowanceService corrosionAllowanceService,
            ICsaClassLocationService csaClassLocationService,
            ICsaHvpLvpService csaHvpLvpService,
            IEpProjectService epProjectService,
            IEpCompanyService epCompanyService,
            ILineListRevisionService lineListRevisionService,
            ISegmentTypeService segmentTypeService,
            IInsulationThicknessService insulationThicknessService,
            IInsulationTypeService insulationTypeService,
            ITracingDesignNumberOfTracersService tracingDesignNumberOfTracersService,
            IInsulationMaterialService insulationMaterialService,
            IPaintSystemService paintSystemService,
            IOperatingModeService operatingModeService,
            IFluidService fluidService,
            IFluidPhaseService fluidPhaseService,
            IPressureProtectionService pressureProtectionService,
            IPipeSpecificationService pipeSpecificationService,
            IPostWeldHeatTreatmentService postWeldHeatTreatmentService,
            IStressAnalysisService stressAnalysisService,
            IInternalCoatingLinerService internalCoatingLinerService,
            ITestMediumService testMediumService,
            IXrayService xrayService,
            INdeCategoryService ndeCategoryService,
            IScheduleService scheduleService,
            ISizeNpsService sizeNpsService,
            ILineStatusService lineStatusService,
            ILineListStatusService lineListStatusService
        )
        {
            _lineRevisionService = lineRevisionService;
            _tracingTypeService = tracingTypeService;
            _lineService = lineService;
            _areaService = areaService;
            _codeService = codeService;
            _commodityService = commodityService;
            _corrosionAllowanceService = corrosionAllowanceService;
            _csaClassLocationService = csaClassLocationService;
            _csaHvpLvpService = csaHvpLvpService;
            _epProjectService = epProjectService;
            _epCompanyService = epCompanyService;
            _lineListRevisionService = lineListRevisionService;
            _segmentTypeService = segmentTypeService;
            _insulationThicknessService = insulationThicknessService;
            _insulationTypeService = insulationTypeService;
            _tracingDesignNumberOfTracersService = tracingDesignNumberOfTracersService;
            _insulationMaterialService = insulationMaterialService;
            _paintSystemService = paintSystemService;
            _operatingModeService = operatingModeService;
            _fluidService = fluidService;
            _fluidPhaseService = fluidPhaseService;
            _pressureProtectionService = pressureProtectionService;
            _pipeSpecificationService = pipeSpecificationService;
            _postWeldHeatTreatmentService = postWeldHeatTreatmentService;
            _stressAnalysisService = stressAnalysisService;
            _internalCoatingLinerService = internalCoatingLinerService;
            _testMediumService = testMediumService;
            _xrayService = xrayService;
            _ndeCategoryService = ndeCategoryService;
            _scheduleService = scheduleService;
            _sizeNpsService = sizeNpsService;
            _lineStatusService = lineStatusService;
            _lineListStatusService = lineListStatusService;
        }

        private static bool SegmentIsJacketed(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            var isJacketed = false;
            var segments = value.Split(new string[] { FormatConstants.NewLine }, StringSplitOptions.None);
            //Jacketed Lines have only 1 segment
            //if (segments.Length == 1)  //SER 296
            //{
            var values = segments[(segments.Length - 1)].Split(':');
            //Jacketed Lines must have an insulation type that is jacketed
            if (values.Length == 2)
            {
                var name = values[1];
                using (var db = new LineListDbContext())
                {
                    var tracingTypes = db.TracingTypes.Where(m => m.Name == name && m.IsJacketed == true);
                    isJacketed = tracingTypes.Any();
                }
            }
            //}
            return isJacketed;
        }

        private static bool SegmentIsJacketed(LineRevision rev)
        {
            var isJacketed = false;
            if (rev.LineRevisionSegments != null)
            {
                isJacketed = rev.LineRevisionSegments.Where(m => m.TracingType != null && m.TracingType.IsJacketed == true).Any();
            }
            return isJacketed;
        }

        public IList<FlatLine> Combine(IList<FlatLine> jacketed)
        {
            var lines = new List<FlatLine>();
            for (int i = 0; i < jacketed.Count; i += 2)
            {
                var annulus = jacketed[i];
                var pipe = jacketed[i + 1];

                if (annulus.Location == pipe.Location
                 && annulus.Commodity == pipe.Commodity
                 && annulus.SequenceNumber == pipe.SequenceNumber
                 && annulus.LineFrom == "GLYCOL JACKET")
                {
                    lines.Add(Combine(annulus, pipe));
                }
                else
                {
                    lines.Add(annulus);
                    lines.Add(pipe);
                }
            }
            return lines;
        }

        private FlatLine Combine(FlatLine annulus, FlatLine pipe)
        {
            pipe.JacketId = annulus.Id;
            pipe.IsJacketed = true;
            pipe.CorrosionAllowance += AppendJacket(annulus.CorrosionAllowance);
            pipe.DesignPressure += AppendJacket(annulus.DesignPressure);
            pipe.DesignMaxTemperature += AppendJacket(annulus.DesignMaxTemperature);
            pipe.DesignMinTemperature += AppendJacket(annulus.DesignMinTemperature);
            pipe.NdeCategory += AppendJacket(annulus.NdeCategory);
            pipe.OperatingPressure += AppendJacket(annulus.OperatingPressure);
            pipe.OperatingTemperature += AppendJacket(annulus.OperatingTemperature);
            pipe.Schedule += AppendJacket(annulus.Schedule);
            pipe.SizeNps += AppendJacket(annulus.SizeNps);
            pipe.TestPressure += AppendJacket(annulus.TestPressure);
            pipe.TestMedium += AppendJacket(annulus.TestMedium);
            pipe.UpsetPressure += AppendJacket(annulus.UpsetPressure);
            pipe.UpsetTemperature += AppendJacket(annulus.UpsetTemperature);
            pipe.WallThickness += AppendJacket(annulus.WallThickness);
            pipe.Xray += AppendJacket(annulus.Xray);

            return pipe;
        }

        private string AppendJacket(string annulusValue)
        {
            if (string.IsNullOrWhiteSpace(annulusValue)) return string.Empty;
            return FormatConstants.NewLine + $"(JK: {annulusValue})";
        }
        private string FlattenSegment(ICollection<LineRevisionSegment> segments, SegmentPropertyEnum property)
        {
            if (segments == null || segments.Count == 0) return string.Empty;

            var sb = new StringBuilder();
            Guid? prevId = Guid.Empty;
            int count = 1;

            var ordered = segments
                .OrderBy(s => s.SegmentType?.Name ?? string.Empty)
                .ThenBy(s => s.SegmentNumber)
                .ToList();

            for (int i = 0; i < ordered.Count; i++)
            {
                var seg = ordered[i];
                if (seg.SegmentTypeId == prevId) count++;
                else count = 1;

                var lookup = GetValue(seg, property);
                var type = seg.SegmentType?.Name ?? string.Empty;

                string formatted;
                if (string.IsNullOrWhiteSpace(type))
                {
                    formatted = lookup;
                }
                else if (count > 1)
                {
                    formatted = $"{type}({count}):{lookup}";
                }
                else if (i + 1 < ordered.Count
                      && ordered[i + 1].SegmentTypeId == seg.SegmentTypeId)
                {
                    formatted = $"{type}({count}):{lookup}";
                }
                else
                {
                    formatted = $"{type}:{lookup}";
                }

                sb.Append(formatted);
                if (i + 1 < ordered.Count) sb.Append(FormatConstants.NewLine);
                prevId = seg.SegmentTypeId;
            }

            return sb.ToString();
        }

        private static string GetValue(LineRevisionSegment seg, SegmentPropertyEnum prop)
        {
            return prop switch
            {
                SegmentPropertyEnum.InsulationThickness => seg.InsulationThickness?.Name ?? string.Empty,
                SegmentPropertyEnum.InsulationType => seg.InsulationType?.Name ?? string.Empty,
                SegmentPropertyEnum.TraceType => seg.TracingType?.Name ?? string.Empty,
                SegmentPropertyEnum.TraceDesignHoldTemperature => seg.TracingDesignHoldTemperature ?? string.Empty,
                SegmentPropertyEnum.TracingDesignNumberOfTracers => seg.TracingDesignNumberOfTracers?.Name ?? string.Empty,
                SegmentPropertyEnum.InsulationMaterial => seg.InsulationMaterial?.Name ?? string.Empty,
                SegmentPropertyEnum.PaintSystem => seg.PaintSystem?.Name ?? string.Empty,
                _ => string.Empty,
            };
        }

        public static IList<FlatLine> ToFlatLines(IList<LineRevision> revs)
        {
            return ToFlatLines(revs, null);
        }

        public static IList<FlatLine> ToFlatLines(IList<LineRevision> revs, Dictionary<string, Dictionary<string, Guid>> lookupCache)
        {
            var lines = new ConcurrentBag<FlatLine>();

                #if DEBUG //when you're debugging, avoid the parrelel threads
                            foreach (var rev in revs)
                #else

                            Parallel.ForEach(revs, rev =>
                #endif
                            {
                                var translated = ToFlatLines(rev, lookupCache);
                                foreach (var line in translated)
                                    lines.Add(line);
                            }

                #if DEBUG

                #else
                );
                #endif
            return lines.ToList();
        }

        public static List<FlatLine> ToFlatLines(LineRevision rev, Dictionary<string, Dictionary<string, Guid>> lookupCache)
        {
            List<FlatLine> items = null;
            using (var db = new LineListDbContext())
            {
                items = ToFlatLines(rev, lookupCache, db);
            }
            return items;
        }

        private static string FlattenSegment(ICollection<LineRevisionSegment> segments, SegmentPropertyEnum property, FlatSourceEnum source)
        {

            StringBuilder value = new StringBuilder(string.Empty);
            string segmentValue = string.Empty;
            string terminator = FormatConstants.NewLine;
            Guid? previousId = Guid.Empty;
            int counter = 1;
            string formatString = "{0}({1}):{2}";
            string formatStringSingle = "{0}:{1}";

            if (segments != null)
            {
                var orderedSegments = segments.OrderBy(m => m.SegmentType == null ? string.Empty : m.SegmentType.Name).ThenBy(m => m.SegmentNumber).ToList();
                var segmentCount = orderedSegments.Count();
                for (int i = 0; i < segmentCount; i++)
                {
                    var segment = orderedSegments[i];
                    counter = segment.SegmentTypeId == previousId ? counter + 1 : 1;
                    string lookupValue = GetValue(segment, property);
                    string segmentType = segment.SegmentType != null ? segment.SegmentType.Name : string.Empty;

                    if (!string.IsNullOrWhiteSpace(segmentType))
                    {
                        if (counter == 1)
                        {
                            if ((i != segmentCount - 1) && (orderedSegments[i].SegmentTypeId == orderedSegments[i + 1].SegmentTypeId))//the next one is the same segment type
                            {
                                segmentValue = string.Format(formatString, segmentType, counter, lookupValue);
                            }
                            else //it's a different type
                                segmentValue = string.Format(formatStringSingle, segmentType, lookupValue);
                        }
                        else
                            segmentValue = string.Format(formatString, segmentType, counter, lookupValue);
                    }
                    else
                        segmentValue = lookupValue;

                    value.Append(segmentValue);

                    if (i < segmentCount - 1)
                        value.Append(terminator);

                    previousId = segment.SegmentTypeId;
                }
            }

            return value.ToString();
        }



        public static List<FlatLine> ToFlatLines(LineRevision rev, Dictionary<string, Dictionary<string, Guid>> lookupCache, LineListDbContext db)
        {
            var cache = lookupCache;
            var list = new List<FlatLine>();


            foreach (var opMode in rev.LineRevisionOperatingModes)
            {
                FlatLine pipe = new FlatLine();

                pipe.Id = rev.Id;
                pipe.Source = FlatSourceEnum.Revision;
                pipe.IsJacketed = SegmentIsJacketed(rev);
                pipe.IsCsa = opMode.Code != null ? opMode.Code.IsCsa : false;
                pipe.ChildNumber = rev.Line.ChildNumber;
                pipe.LineListRevisionId = rev.LineListRevisionId;

                pipe.Checked = rev.CheckedOutBy;
                pipe.RequiresValidation = rev.RequiresMinimumInformation;
                //item.Checked = rev.Checked; //IGNORE ACCORDING TO SPEC
                //item.Duplicate = rev.Duplicate; //IGNORE ACCORDING TO SPEC
                pipe.Deleted = (rev.LineStatus != null && rev.LineStatus.Name == "Deleted") ? FormatConstants.Yes : string.Empty;
                pipe.ReservedBy = (rev.LineStatus != null && rev.LineStatus.Name == "Reserved") ? FormatConstants.Yes : string.Empty;
                pipe.AltOpMode = opMode.OperatingModeNumber;

                pipe.OpMode = opMode != null && opMode.OperatingMode != null ? opMode.OperatingMode.Name : null;
                pipe.LineFrom = opMode != null ? opMode.LineRoutingFrom : null;
                pipe.LineTo = opMode != null ? opMode.LineRoutingTo : null;
                pipe.FluidPhase = (opMode != null && opMode.FluidPhase != null) ? opMode.FluidPhase.Name : string.Empty;
                pipe.OperatingPressure = opMode != null ? opMode.OperatingPressurePipe : null;
                pipe.OperatingTemperature = opMode != null ? opMode.OperatingTemperaturePipe : null;
                pipe.Fluid = opMode != null && opMode.Fluid != null ? opMode.Fluid.Name : null;
                pipe.CsaClassLocation = opMode != null && opMode.CsaClassLocation != null ? opMode.CsaClassLocation.Name : null;
                pipe.CsaHvpLvp = opMode != null && opMode.CsaHvpLvp != null ? opMode.CsaHvpLvp.Name : null;
                pipe.PipeMaterialSpecification = opMode != null ? opMode.PipeMaterialSpecification : null;
                pipe.HoopStressLevel = opMode != null && opMode.HoopStressLevel != null ? opMode.HoopStressLevel.Value.ToString("0.0") : string.Empty;
                pipe.SourService = opMode != null && opMode.Code != null && opMode.Code.IsCsa ? opMode.IsSourService.Value ? FormatConstants.Yes : FormatConstants.No : string.Empty;
                pipe.Notes = opMode != null ? opMode.Notes : null;
                pipe.Code = (opMode != null && opMode.Code != null) ? opMode.Code.Name : string.Empty;
                pipe.ParentChild = rev.Line.ChildNumber != 0 ? Relationship.Child : db.Lines.Where(m => m.LocationId == rev.Line.LocationId && m.CommodityId == rev.Line.CommodityId && m.SequenceNumber == rev.Line.SequenceNumber && m.ChildNumber != 0).Any() ? Relationship.Parent : string.Empty;

                pipe.Specification = string.Empty;
                if (rev.Specification != null)
                    pipe.Specification = rev.Specification.Name;
                else
                    if (rev.LineListRevision != null)
                    if (rev.LineListRevision.Specification != null)
                        pipe.Specification = rev.LineListRevision.Specification.Name;

                pipe.Location = string.Empty;
                if (rev.Line.Location != null)
                    pipe.Location = rev.Line.Location.Name;
                else
                    if (rev.LineListRevision != null)
                    if (rev.LineListRevision.Location != null)
                        pipe.Location = rev.LineListRevision.Location.Name;

                pipe.Commodity = (rev.Line.Commodity != null) ? rev.Line.Commodity.Name : string.Empty;
                pipe.SequenceNumber = rev.Line.SequenceNumber;
                pipe.DocumentNumber = rev.LineListRevision.LineListDocumentNumber;
                pipe.ModularId = (rev.Line.ModularId != null) ? rev.Line.ModularId : string.Empty;

                pipe.AbsaRegistration = (opMode != null) ? opMode.IsAbsaRegistration : string.Empty; //opMode != null && opMode.IsAbsaRegistration ? FormatConstants.Yes : FormatConstants.No; //SER 435.  Changes by Armando Chaves.
                pipe.PressureProtection = (opMode != null && opMode.PressureProtection != null) ? opMode.PressureProtection.Name : string.Empty;//SER 435.  Changes by Armando Chaves.

                if (opTokens.Contains(opMode.OperatingModeNumber))
                {
                    pipe.Area = (rev.Area != null) ? rev.Area.Name : string.Empty;

                    pipe.ClassServMaterial = (rev.PipeSpecification != null) ? rev.PipeSpecification.Name : string.Empty;
                    pipe.SizeNps = (rev.SizeNpsPipe != null) ? rev.SizeNpsPipe.DecimalValue : string.Empty;

                    pipe.InsulationThickness = FlattenSegment(rev.LineRevisionSegments, SegmentPropertyEnum.InsulationThickness, pipe.Source);
                    pipe.InsulationType = FlattenSegment(rev.LineRevisionSegments, SegmentPropertyEnum.InsulationType, pipe.Source);
                    pipe.TraceDesignType = FlattenSegment(rev.LineRevisionSegments, SegmentPropertyEnum.TraceType, pipe.Source);
                    pipe.TraceDesignHoldTemp = FlattenSegment(rev.LineRevisionSegments, SegmentPropertyEnum.TraceDesignHoldTemperature, pipe.Source);
                    pipe.TracingDesignNumTracers = FlattenSegment(rev.LineRevisionSegments, SegmentPropertyEnum.TracingDesignNumberOfTracers, pipe.Source);
                    pipe.InsulationMaterial = FlattenSegment(rev.LineRevisionSegments, SegmentPropertyEnum.InsulationMaterial, pipe.Source);
                    pipe.PaintSystem = FlattenSegment(rev.LineRevisionSegments, SegmentPropertyEnum.PaintSystem, pipe.Source);

                    var seg = rev.LineRevisionSegments != null && rev.LineRevisionSegments.Where(m => m.SegmentNumber == "1").Count() > 0 ? rev.LineRevisionSegments.Where(m => m.SegmentNumber == "1").First() : null;

                    pipe.SegmentNumber = seg != null ? seg.SegmentNumber : null;
                    pipe.SegmentType = (seg != null && seg.SegmentType != null) ? seg.SegmentType.Name : null;
                    pipe.OriginatingPID = rev.OriginatingPID;
                    pipe.Schedule = (rev.SchedulePipe != null) ? rev.SchedulePipe.Name : string.Empty;
                    pipe.DesignPressure = rev.DesignPressurePipe;
                    pipe.DesignMaxTemperature = rev.DesignTemperatureMaximumPipe;
                    pipe.DesignMinTemperature = rev.DesignTemperatureMinimumPipe;
                    pipe.TestPressure = rev.TestPressurePipe;
                    pipe.TestMedium = (rev.TestMediumPipe != null) ? rev.TestMediumPipe.Name : string.Empty;
                    pipe.ExpTemperature = rev.ExpansionTemperature;
                    pipe.UpsetPressure = rev.UpsetPressurePipe;
                    pipe.UpsetTemperature = rev.UpsetTemperaturePipe;
                    pipe.MdmtTemperature = rev.MinimumDesignMetalTemperature;
                    pipe.CorrosionAllowance = (rev.CorrosionAllowancePipe != null) ? rev.CorrosionAllowancePipe.Name : string.Empty;
                    pipe.Xray = (rev.XrayPipe != null) ? rev.XrayPipe.Name : string.Empty;
                    pipe.NdeCategory = (rev.NdeCategoryPipe != null) ? rev.NdeCategoryPipe.Name : string.Empty;
                    pipe.PostWeldHeatTreatment = (rev.PostWeldHeatTreatment != null) ? rev.PostWeldHeatTreatment.Name : string.Empty;
                    pipe.StressAnalysis = (rev.StressAnalysis != null) ? rev.StressAnalysis.Name : string.Empty;
                    pipe.WallThickness = Convert.ToString(rev.WallThicknessPipe);
                    pipe.pipeWallThickness = Convert.ToString(rev.WallThicknessPipe);
                    pipe.InternalCoatingLiner = (rev.InternalCoatingLiner != null) ? rev.InternalCoatingLiner.Name : string.Empty;
                    //pipe.AbsaRegistration = (opMode != null) ? opMode.IsAbsaRegistration : string.Empty; //opMode != null && opMode.IsAbsaRegistration ? FormatConstants.Yes : FormatConstants.No; //SER 435.  Changes by Armando Chaves.
                    // pipe.PressureProtection = (opMode.PressureProtection != null && opMode.PressureProtection != null) ? opMode.PressureProtection.Name : string.Empty;//SER 435.  Changes by Armando Chaves.
                    pipe.LineStatus = rev.LineStatus != null && rev.LineStatus != null ? rev.LineStatus.Name : string.Empty;
                    pipe.LineRevision = rev.Revision;

                    if (rev.LineListRevision != null)
                    {
                        pipe.DocumentRevision = rev.LineListRevision.DocumentRevision;
                        pipe.IssuedOn = rev.LineListRevision.IssuedOn.Value.ToString(FormatConstants.DateFormat);
                        if (rev.LineListRevision.LineList != null)
                            pipe.DocumentNumber = rev.LineListRevision.LineList.DocumentNumber;

                        if (rev.LineListRevision.EpProject == null)
                            pipe.EpProject = GetCacheValue(cache, "EpProject", rev.LineListRevision.EpProjectId);
                        else
                            pipe.EpProject = rev.LineListRevision.EpProject.Name;

                        if (rev.LineListRevision.EpCompany == null)
                            pipe.EpCompany = GetCacheValue(cache, "EpCompany", rev.LineListRevision.EpCompanyId);
                        else
                            pipe.EpCompany = rev.LineListRevision.EpCompany.Name;
                    }

                    if (rev.LineStatus != null)
                        pipe.AsBuilt = rev.LineStatus.Name.ToUpperInvariant().Contains("AS BUILT") ? rev.LineStatus.Name : string.Empty;

                    if (pipe.IsJacketed)
                    {
                        pipe.CorrosionAllowance += (rev.CorrosionAllowancePipe != null ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.CorrosionAllowanceAnnulus != null ? rev.CorrosionAllowanceAnnulus.Name : string.Empty);
                        pipe.DesignPressure += (rev.DesignPressurePipe != null ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.DesignPressureAnnulus);
                        pipe.DesignMaxTemperature += (!string.IsNullOrWhiteSpace(rev.DesignTemperatureMaximumPipe) ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.DesignTemperatureMaximumAnnulus);
                        pipe.DesignMinTemperature += (!string.IsNullOrWhiteSpace(rev.DesignTemperatureMinimumPipe) ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.DesignTemperatureMinimumAnnulus);
                        pipe.NdeCategory += (rev.CorrosionAllowancePipe != null ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.NdeCategoryAnnulus != null ? rev.NdeCategoryAnnulus.Name : string.Empty);
                        pipe.OperatingPressure += (!string.IsNullOrWhiteSpace(opMode.OperatingPressurePipe) ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", opMode.OperatingPressureAnnulus);
                        pipe.OperatingTemperature += (!string.IsNullOrWhiteSpace(opMode.OperatingTemperaturePipe) ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", opMode.OperatingTemperatureAnnulus);
                        pipe.Schedule += (rev.SchedulePipe == null && rev.ScheduleAnnulus == null) ? string.Empty : (rev.SchedulePipe != null ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.ScheduleAnnulus != null ? rev.ScheduleAnnulus.Name : string.Empty);

                        pipe.annuSchedule = rev.ScheduleAnnulus != null ? rev.ScheduleAnnulus.Name : string.Empty;
                        pipe.pipeSchedule = rev.SchedulePipe != null ? rev.SchedulePipe.Name : string.Empty;

                        pipe.SizeNps += (rev.SizeNpsPipe != null ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.SizeNpsAnnulus != null ? rev.SizeNpsAnnulus.DecimalValue : string.Empty);
                        pipe.TestPressure += (!string.IsNullOrWhiteSpace(rev.TestPressurePipe) ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.TestPressureAnnulus);
                        pipe.TestMedium += (rev.TestMediumPipe != null ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.TestMediumAnnulus != null ? rev.TestMediumAnnulus.Name : string.Empty);
                        pipe.UpsetPressure += (!string.IsNullOrWhiteSpace(rev.UpsetPressurePipe) ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.UpsetPressureAnnulus);
                        pipe.UpsetTemperature += (!string.IsNullOrWhiteSpace(rev.UpsetTemperaturePipe) ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.UpsetTemperatureAnnulus);
                        pipe.WallThickness += (rev.WallThicknessPipe == null && rev.WallThicknessAnnulus == null) ? string.Empty : (rev.WallThicknessPipe != null ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", rev.WallThicknessAnnulus != null ? rev.WallThicknessAnnulus.ToString() : string.Empty);

                        pipe.annuWallThickness = rev.WallThicknessAnnulus.HasValue ? string.Format("{0}", rev.WallThicknessAnnulus) : string.Empty;
                        pipe.pipeWallThickness = rev.WallThicknessPipe.HasValue ? string.Format("{0}", rev.WallThicknessPipe) : string.Empty;

                        pipe.Xray += (rev.XrayPipe != null ? FormatConstants.NewLine : string.Empty) + string.Format("(JK: {0})", (rev.XrayAnnulus != null) ? rev.XrayAnnulus.Name : string.Empty);
                    }
                }
                list.Add(pipe);
            }
            return list;

        }

        public IList<FlatLine> ToFlatLines(IList<ImportRow> rows)
        {
            var bag = new ConcurrentBag<FlatLine>();
            Parallel.ForEach(rows, row =>
            {
                var list = ToFlatLines(row, _tracingTypeService);
                foreach (var fl in list) bag.Add(fl);
            });

            return bag.Any(m => m.RowNumber > 0)
                ? bag.OrderBy(m => m.RowNumber).ToList()
                : bag.OrderBy(m => m.Location)
                     .ThenBy(m => m.Commodity)
                     .ThenBy(m => m.SequenceNumber)
                     .ThenBy(m => m.ChildNumber)
                     .ToList();
        }



        public List<FlatLine> ToFlatLines(ImportRow row, ITracingTypeService tracingTypeService)
        {
            var pipe = new FlatLine();
            var annulus = (FlatLine)null;
            var list = new List<FlatLine>();

            // --- copy your old static implementation here, e.g.:
            pipe.Id = row.Id;
            pipe.Source = FlatSourceEnum.Import;
            pipe.IsJacketed = SegmentIsJacketed(row.excelTraceDesignType);
            pipe.RowNumber = row.RowNumber;
            pipe.Checked = row.excelChecked ?? string.Empty;
            // …and all the rest of the row-to-FlatLine mapping…
            // (you can literally copy/paste from your original FlatFactory static code)

            if (annulus != null) list.Add(annulus);
            list.Add(pipe);
            return list;
        }

       
        public IList<LineRevision> ToLineRevisions(
            IList<FlatLine> values,
            string userName,
            bool isJacketed,
            bool updateValidation)
        {
            // derive facility from EpProject/EpCompany if present
            string facility = string.Empty;
            var withProj = values.FirstOrDefault(m => !string.IsNullOrWhiteSpace(m.EpProject));
            if (withProj != null)
            {
                var proj = _epProjectService.GetAll().Result
                    .First(p => p.Name == withProj.EpProject);
                facility = proj.CenovusProject.Facility.Name;
            }

            return ToLineRevisions(values, userName, facility, null, isJacketed, updateValidation, null);
        }

    

        public IList<LineRevision> ToLineRevisions(
            IList<FlatLine> values,
            string userName,
            string facility,
            Dictionary<string, Dictionary<string, Guid>> lookupCache,
            bool isJacketed,
            bool updateValidation,
            Dictionary<string, Guid> lineListIds)
        {
            _lineListRevisionIds = lineListIds;
            return ToLineRevisions(values, userName, facility, lookupCache, isJacketed, updateValidation);
        }

        public IList<LineRevision> ToLineRevisions(
            IList<FlatLine> values,
            string userName,
            string facility,
            Dictionary<string, Dictionary<string, Guid>> lookupCache,
            bool isJacketed,
            bool updateValidation)
        {
            unnumberedChildren = new List<string>();
            var bag = new ConcurrentBag<LineRevision>();
            bool fromImport = values.Count > 0 && values[0].Source == FlatSourceEnum.Import;

            if (isJacketed && fromImport)
            {
                // pair pipe+annulus imports
                var groups = values
                    .Select(m => new { m.DocumentNumber, m.DocumentRevision, m.Location, m.Commodity, m.SequenceNumber })
                    .Distinct();

                foreach (var g in groups)
                {
                    var pair = values.Where(m =>
                           m.DocumentNumber == g.DocumentNumber
                        && m.DocumentRevision == g.DocumentRevision
                        && m.Location == g.Location
                        && m.Commodity == g.Commodity
                        && m.SequenceNumber == g.SequenceNumber)
                        .ToList();

                    if (pair.Count == 2)
                    {
                        // first = annulus, second = pipe
                        bag.Add(ToLineRevision(pair[1], new List<FlatLine>(), userName, lookupCache, facility, pair[0], updateValidation));
                    }
                }
            }
            else
            {
                // normal case
                foreach (var id in values.Select(m => m.Id).Distinct())
                {
                    var group = values.Where(m => m.Id == id).ToList();
                    var primary = group.First(m => opTokens.Contains(m.AltOpMode));
                    var alternates = group.Except(new[] { primary }).ToList();
                    bag.Add(ToLineRevision(primary, alternates, userName, lookupCache, facility, null, updateValidation));
                }
            }

            return bag.ToList();
        }

        public LineRevision ToLineRevision(
            FlatLine value,
            List<FlatLine> altOpModes,
            string userName,
            Dictionary<string, Dictionary<string, Guid>> cache,
            string facility,
            FlatLine annulus,
            bool updateValidationState)
        {
            // fetch or new
            var allRevs = _lineRevisionService.GetAll().Result;
            bool newRow = value.Id == Guid.Empty || value.Source == FlatSourceEnum.Import;
            LineRevision item = null;

            if (!newRow)
            {
                item = allRevs.FirstOrDefault(r => r.Id == value.Id);
                if (item == null) newRow = true;
            }

            if (newRow)
            {
                item = new LineRevision
                {
                    Id = value.Id == Guid.Empty ? Guid.NewGuid() : value.Id,
                    CreatedBy = userName,
                    CreatedOn = DateTime.Now,
                    LineRevisionSegments = new List<LineRevisionSegment>(),
                    LineRevisionOperatingModes = new List<LineRevisionOperatingMode>(),
                    IsActive = true,
                    IsReferenceLine = false,
                    RequiresMinimumInformation = !string.IsNullOrWhiteSpace(value.ReservedBy)
                };

                // link LineListRevision
                if (_lineListRevisionIds != null && _lineListRevisionIds.ContainsKey(value.DocumentNumber))
                    item.LineListRevisionId = _lineListRevisionIds[value.DocumentNumber];
                else if (!string.IsNullOrWhiteSpace(value.DocumentNumber))
                    item.LineListRevisionId = GetCacheValue(cache, "LineListRevision", value.DocumentNumber.ToUpperInvariant());

                _lineRevisionService.Add(item);
            }

            // translate operating modes
            if (altOpModes != null && altOpModes.Any())
                TranslateLineRevisionOperatingMode(value, altOpModes, item, userName, cache);
            else
                TranslateLineRevisionOperatingMode(value, item, userName, cache);

            // only copy primary altOpMode onto main record
            if (opTokens.Contains(value.AltOpMode))
            {
                // facility fallback
                if (string.IsNullOrWhiteSpace(facility) && item.LineListRevision?.Location?.Facility != null)
                    facility = item.LineListRevision.Location.Facility.Name;

                item.ModifiedBy = userName;
                item.ModifiedOn = DateTime.Now;
                item.Revision = value.LineRevision;

                // area
                item.AreaId = string.IsNullOrWhiteSpace(value.Area)
                                   ? (Guid?)null
                                   : GetCacheValue(cache, "Area", value.Area.Trim().ToUpperInvariant()
                                                      + FormatConstants.Delimiter
                                                      + value.Location.Trim().ToUpperInvariant()
                                                      + FormatConstants.Delimiter
                                                      + value.Specification.Trim().ToUpperInvariant());

                // specs
                item.SpecificationId = string.IsNullOrWhiteSpace(value.Specification)
                                     ? (Guid?)null
                                     : GetCacheValue(cache, "Specification", value.Specification.Trim().ToUpperInvariant());

                item.PipeSpecificationId = string.IsNullOrWhiteSpace(value.ClassServMaterial)
                                          ? (Guid?)null
                                          : GetCacheValue(cache, "PipeSpecification", value.ClassServMaterial.Trim().ToUpperInvariant()
                                                             + FormatConstants.Delimiter
                                                             + value.Specification.Trim().ToUpperInvariant());

                // annulus vs pipe values
                if (annulus != null)
                {
                    // annulus ids & fields
                    item.CorrosionAllowanceAnnulusId = string.IsNullOrWhiteSpace(annulus.CorrosionAllowance)
                                                         ? (Guid?)null
                                                         : GetCacheValue(cache, "CorrossionAllowance", annulus.CorrosionAllowance.Trim().ToUpperInvariant());
                    item.DesignPressureAnnulus = annulus.DesignPressure?.Trim();
                    item.DesignTemperatureMaximumAnnulus = annulus.DesignMaxTemperature?.Trim();
                    item.DesignTemperatureMinimumAnnulus = annulus.DesignMinTemperature?.Trim();
                    item.NdeCategoryAnnulusId = string.IsNullOrWhiteSpace(annulus.NdeCategory)
                                                         ? (Guid?)null
                                                         : GetCacheValue(cache, "NdeCategory", annulus.NdeCategory.Trim().ToUpperInvariant());
                    item.ScheduleAnnulusId = string.IsNullOrWhiteSpace(annulus.Schedule)
                                                         ? (Guid?)null
                                                         : GetCacheValue(cache, "Schedule", annulus.Schedule.Trim().ToUpperInvariant());
                    item.SizeNpsAnnulusId = string.IsNullOrWhiteSpace(annulus.SizeNps)
                                                         ? (Guid?)null
                                                         : GetCacheValue(cache, "SizeNps", annulus.SizeNps.Trim().ToUpperInvariant());
                    item.TestPressureAnnulus = annulus.TestPressure?.Trim();
                    item.TestMediumAnnulusId = string.IsNullOrWhiteSpace(annulus.TestMedium)
                                                         ? (Guid?)null
                                                         : GetCacheValue(cache, "TestMedium", annulus.TestMedium.Trim().ToUpperInvariant());
                    item.UpsetPressureAnnulus = annulus.UpsetPressure?.Trim();
                    item.UpsetTemperatureAnnulus = annulus.UpsetTemperature?.Trim();
                    item.WallThicknessAnnulus = string.IsNullOrWhiteSpace(annulus.WallThickness)
                                                         ? (decimal?)null
                                                         : Decimal.TryParse(annulus.WallThickness, out var aVal) ? aVal : (decimal?)null;
                    item.XrayAnnulusId = string.IsNullOrWhiteSpace(annulus.Xray)
                                                         ? (Guid?)null
                                                         : GetCacheValue(cache, "Xray", annulus.Xray.Trim().ToUpperInvariant());

                    // copy to first op‐mode in collection
                    var op0 = item.LineRevisionOperatingModes.OrderBy(m => m.OperatingModeNumber).FirstOrDefault();
                    if (op0 != null)
                    {
                        op0.OperatingPressureAnnulus = annulus.OperatingPressure;
                        op0.OperatingTemperatureAnnulus = annulus.OperatingTemperature;
                    }
                }

                // pipe values
                item.CorrosionAllowancePipeId = string.IsNullOrWhiteSpace(value.CorrosionAllowance)
                                                     ? (Guid?)null
                                                     : GetCacheValue(cache, "CorrossionAllowance", value.CorrosionAllowance.Trim().ToUpperInvariant());
                item.DesignPressurePipe = value.DesignPressure?.Trim();
                item.DesignTemperatureMaximumPipe = value.DesignMaxTemperature?.Trim();
                item.DesignTemperatureMinimumPipe = value.DesignMinTemperature?.Trim();
                item.NdeCategoryPipeId = string.IsNullOrWhiteSpace(value.NdeCategory)
                                                     ? (Guid?)null
                                                     : GetCacheValue(cache, "NdeCategory", value.NdeCategory.Trim().ToUpperInvariant());
                item.SchedulePipeId = string.IsNullOrWhiteSpace(value.Schedule)
                                                     ? (Guid?)null
                                                     : GetCacheValue(cache, "Schedule", value.Schedule.Trim().ToUpperInvariant());
                item.SizeNpsPipeId = string.IsNullOrWhiteSpace(value.SizeNps)
                                                     ? (Guid?)null
                                                     : GetCacheValue(cache, "SizeNps", value.SizeNps.Trim().ToUpperInvariant());
                item.TestPressurePipe = value.TestPressure?.Trim();
                item.TestMediumPipeId = string.IsNullOrWhiteSpace(value.TestMedium)
                                                     ? (Guid?)null
                                                     : GetCacheValue(cache, "TestMedium", value.TestMedium.Trim().ToUpperInvariant());
                item.UpsetPressurePipe = value.UpsetPressure?.Trim();
                item.UpsetTemperaturePipe = value.UpsetTemperature?.Trim();
                item.WallThicknessPipe = string.IsNullOrWhiteSpace(value.WallThickness)
                                                     ? (decimal?)null
                                                     : Decimal.TryParse(value.WallThickness, out var pVal) ? pVal : (decimal?)null;
                item.XrayPipeId = string.IsNullOrWhiteSpace(value.Xray)
                                                     ? (Guid?)null
                                                     : GetCacheValue(cache, "Xray", value.Xray.Trim().ToUpperInvariant());

                // copy to first op‐mode in collection
                var opMain = item.LineRevisionOperatingModes.OrderBy(m => m.OperatingModeNumber).FirstOrDefault();
                if (opMain != null)
                {
                    opMain.OperatingPressurePipe = value.OperatingPressure;
                    opMain.OperatingTemperaturePipe = value.OperatingTemperature;
                }
            }

            // segments (pipe+annulus)
            TranslateSegments(value, item, userName, cache, annulus != null);

            // other misc fields
            item.OriginatingPID = value.OriginatingPID?.Trim();
            item.ExpansionTemperature = value.ExpTemperature?.Trim();
            item.MinimumDesignMetalTemperature = value.MdmtTemperature?.Trim();
            item.PostWeldHeatTreatmentId = string.IsNullOrWhiteSpace(value.PostWeldHeatTreatment)
                                             ? (Guid?)null
                                             : GetCacheValue(cache, "PostWeldHeatTreatment", value.PostWeldHeatTreatment.Trim().ToUpperInvariant());
            item.StressAnalysisId = string.IsNullOrWhiteSpace(value.StressAnalysis)
                                             ? (Guid?)null
                                             : GetCacheValue(cache, "StressAnalysis", value.StressAnalysis.Trim().ToUpperInvariant());
            item.InternalCoatingLinerId = string.IsNullOrWhiteSpace(value.InternalCoatingLiner)
                                             ? (Guid?)null
                                             : GetCacheValue(cache, "InternalCoatingLiner", value.InternalCoatingLiner.Trim().ToUpperInvariant());

            // line numbering & status
            if (item.Line != null)
            {
                item.LineNumber = LineNumberGenerator.Evaluate(
                                    item,
                                    _locationService,
                                    _commodityService,
                                    _pipeSpecificationService,
                                    _sizeNpsService,
                                    _insulationThicknessService,
                                    _tracingTypeService,
                                    _insulationTypeService
                                );
                item.Line.ModularId = value.ModularId?.Trim();
            }

            // assign status
            item.LineStatusId = GetLineStatus(item, cache, value);

            // validation
            if (updateValidationState)
                item.ValidationState = (int)LineRevisionValidation.GetHardRevisionValidationState(item);

            return item;
        }

        private void TranslateLineRevisionOperatingMode(
            FlatLine flat,
            LineRevision rev,
            string userName,
            Dictionary<string, Dictionary<string, Guid>> cache)
        {
            var modes = rev.LineRevisionOperatingModes;
            var opMode = modes.FirstOrDefault(m => m.OperatingModeNumber == flat.AltOpMode)
                         ?? new LineRevisionOperatingMode { Id = Guid.NewGuid(), CreatedBy = userName, CreatedOn = DateTime.Now, LineRevision = rev };

            opMode.ModifiedBy = userName;
            opMode.ModifiedOn = DateTime.Now;
            opMode.LineRoutingFrom = flat.LineFrom;
            opMode.LineRoutingTo = flat.LineTo;
            opMode.OperatingModeNumber = string.IsNullOrWhiteSpace(flat.AltOpMode) ? "1" : flat.AltOpMode;
            opMode.CodeId = string.IsNullOrWhiteSpace(flat.Code)
                                             ? (Guid?)null
                                             : GetCacheValue(cache, "Code", flat.Code.Trim().ToUpperInvariant());
            opMode.IsAbsaRegistration = flat.AbsaRegistration;
            opMode.HoopStressLevel = string.IsNullOrWhiteSpace(flat.HoopStressLevel) ? (decimal?)null : Decimal.Parse(flat.HoopStressLevel);
            opMode.CsaClassLocationId = string.IsNullOrWhiteSpace(flat.CsaClassLocation)
                                             ? (Guid?)null
                                             : GetCacheValue(cache, "CsaClassLocation", flat.CsaClassLocation.Trim().ToUpperInvariant());
            opMode.IsSourService = (FormatConstants.Yes == flat.SourService);
            opMode.CsaHvpLvpId = string.IsNullOrWhiteSpace(flat.CsaHvpLvp)
                                             ? (Guid?)null
                                             : GetCacheValue(cache, "CsaHvpLvp", flat.CsaHvpLvp.Trim().ToUpperInvariant());
            opMode.FluidId = string.IsNullOrWhiteSpace(flat.Fluid)
                                             ? (Guid?)null
                                             : GetCacheValue(cache, "Fluid", flat.Fluid.Trim().ToUpperInvariant());
            opMode.FluidPhaseId = string.IsNullOrWhiteSpace(flat.FluidPhase)
                                             ? (Guid?)null
                                             : GetCacheValue(cache, "FluidPhase", flat.FluidPhase.Trim().ToUpperInvariant());
            opMode.OperatingPressurePipe = flat.OperatingPressure;
            opMode.OperatingTemperaturePipe = flat.OperatingTemperature;
            opMode.PipeMaterialSpecification = flat.PipeMaterialSpecification;
            opMode.PressureProtection = string.IsNullOrWhiteSpace(flat.PressureProtection)
                                             ? null
                                            : _pressureProtectionService.GetAll().Result.FirstOrDefault(p => p.Name.Equals(flat.PressureProtection, StringComparison.OrdinalIgnoreCase));
            opMode.ModularId = flat.ModularId;

            // ensure in collection
            if (!modes.Contains(opMode))
                rev.LineRevisionOperatingModes.Add(opMode);
        }

        private void TranslateLineRevisionOperatingMode(
            FlatLine flat,
            List<FlatLine> alts,
            LineRevision rev,
            string userName,
            Dictionary<string, Dictionary<string, Guid>> cache)
        {
            foreach (var alt in alts)
                TranslateLineRevisionOperatingMode(alt, rev, userName, cache);
        }

        private void TranslateSegments(
            FlatLine flat,
            LineRevision rev,
            string userName,
            Dictionary<string, Dictionary<string, Guid>> cache,
            bool hasAnnulus)
        {
            var sep = new[] { FormatConstants.NewLine };
            var defaultType = "I/S";
            var insulList = flat.InsulationThickness?.Split(sep, StringSplitOptions.None) ?? new[] { "" };
            var typeList = flat.InsulationType?.Split(sep, StringSplitOptions.None) ?? new[] { "" };
            var traceList = flat.TraceDesignType?.Split(sep, StringSplitOptions.None) ?? new[] { "" };
            var numList = flat.TracingDesignNumTracers?.Split(sep, StringSplitOptions.None) ?? new[] { "" };
            var holdList = flat.TraceDesignHoldTemp?.Split(sep, StringSplitOptions.None) ?? new[] { "" };
            var matList = flat.InsulationMaterial?.Split(sep, StringSplitOptions.None) ?? new[] { "" };
            var paintList = flat.PaintSystem?.Split(sep, StringSplitOptions.None) ?? new[] { "" };

            var now = DateTime.Now;
            var segments = rev.LineRevisionSegments ?? new List<LineRevisionSegment>();
            rev.LineRevisionSegments = segments;

            for (int i = 0; i < insulList.Length; i++)
            {
                // parse each segment’s properties
                string segType = "";
                string segValue = "";
                string segNumber = (i + 1).ToString();
                string insType = "";
                string trcType = "";
                string trcNum = "";
                string trcHold = "";
                string insMat = "";
                string paintSys = "";

                // thickness
                var t = insulList[i];
                if (t.Contains(":"))
                {
                    var parts = t.Split(':');
                    segType = hasAnnulus ? "" : parts[0].Trim().ToUpperInvariant();
                    segValue = parts[1].Trim().ToUpperInvariant();
                }
                else
                    segValue = t.Trim().ToUpperInvariant();

                // insulation type
                if (typeList.Length > i && !string.IsNullOrWhiteSpace(typeList[i]))
                    insType = typeList[i].Contains(":")
                              ? typeList[i].Split(':')[1].Trim().ToUpperInvariant()
                              : typeList[i].Trim().ToUpperInvariant();

                // tracer type
                if (traceList.Length > i && !string.IsNullOrWhiteSpace(traceList[i]))
                {
                    var raw = traceList[i].Contains(":") ? traceList[i].Split(':')[1] : traceList[i];
                    trcType = raw.Trim().ToUpperInvariant();
                }

                // tracer count
                if (numList.Length > i && !string.IsNullOrWhiteSpace(numList[i]))
                    trcNum = numList[i].Contains(":") ? numList[i].Split(':')[1].Trim().ToUpperInvariant() : numList[i].Trim().ToUpperInvariant();

                // hold temp
                if (holdList.Length > i && !string.IsNullOrWhiteSpace(holdList[i]))
                    trcHold = holdList[i].Contains(":") ? holdList[i].Split(':')[1].Trim().ToUpperInvariant() : holdList[i].Trim().ToUpperInvariant();

                // insul material
                if (matList.Length > i && !string.IsNullOrWhiteSpace(matList[i]))
                    insMat = matList[i].Contains(":") ? matList[i].Split(':')[1].Trim().ToUpperInvariant() : matList[i].Trim().ToUpperInvariant();

                // paint
                if (paintList.Length > i && !string.IsNullOrWhiteSpace(paintList[i]))
                    paintSys = paintList[i].Contains(":") ? paintList[i].Split(':')[1].Trim().ToUpperInvariant() : paintList[i].Trim().ToUpperInvariant();

                // jacket override: if jacketed, always use JGL
                if (flat.IsJacketed)
                    trcType = _tracingTypeService.GetAll().Result.First(t2 => t2.IsJacketed && t2.Name == "JGL").Name;

                // find or new segment
                var segment = segments.FirstOrDefault(s => s.SegmentNumber == segNumber)
                              ?? new LineRevisionSegment { Id = Guid.NewGuid(), LineRevisionId = rev.Id, CreatedBy = userName, CreatedOn = now, SegmentNumber = segNumber };

                segment.ModifiedBy = userName;
                segment.ModifiedOn = now;

                // cleanup hyphens
                segType = segType.Trim() == "-" ? (flat.Source == FlatSourceEnum.Import && !string.IsNullOrWhiteSpace(flat.ReservedBy) ? defaultType : "") : segType;
                segValue = segValue.Trim() == "-" ? "" : segValue;
                insType = insType.Trim() == "-" ? "" : insType;
                trcType = trcType.Trim() == "-" ? "" : trcType;
                trcNum = trcNum.Trim() == "-" ? "" : trcNum;
                trcHold = trcHold.Trim() == "-" ? "" : trcHold;
                insMat = insMat.Trim() == "-" ? "" : insMat;
                paintSys = paintSys.Trim() == "-" ? "" : paintSys;

                // assign lookups
                segment.SegmentTypeId = string.IsNullOrWhiteSpace(segType) ? (Guid?)null : GetCacheValue(cache, "SegmentType", segType);
                segment.InsulationThicknessId = string.IsNullOrWhiteSpace(segValue) ? (Guid?)null : GetCacheValue(cache, "InsulationThickness", segValue);
                segment.InsulationTypeId = string.IsNullOrWhiteSpace(insType) ? (Guid?)null : GetCacheValue(cache, "InsulationType", insType);
                segment.TracingTypeId = string.IsNullOrWhiteSpace(trcType) ? (Guid?)null : GetCacheValue(cache, "TraceType", trcType + FormatConstants.Delimiter + flat.Specification);
                segment.TracingDesignNumberOfTracersId = string.IsNullOrWhiteSpace(trcNum) ? (Guid?)null : GetCacheValue(cache, "TracingDesignNumberOfTracers", trcNum);
                segment.TracingDesignHoldTemperature = trcHold;
                segment.InsulationMaterialId = string.IsNullOrWhiteSpace(insMat) ? (Guid?)null : GetCacheValue(cache, "InsulationMaterial", insMat);
                segment.PaintSystemId = string.IsNullOrWhiteSpace(paintSys) ? (Guid?)null : GetCacheValue(cache, "PaintSystem", paintSys);

                // refresh nav props
                segment.SegmentType = segment.SegmentTypeId.HasValue ? _segmentTypeService.GetAll().Result.Single(m => m.Id == segment.SegmentTypeId) : null;
                segment.InsulationThickness = segment.InsulationThicknessId.HasValue ? _insulationThicknessService.GetAll().Result.Single(m => m.Id == segment.InsulationThicknessId) : null;
                segment.InsulationType = segment.InsulationTypeId.HasValue ? _insulationTypeService.GetAll().Result.Single(m => m.Id == segment.InsulationTypeId) : null;
                segment.TracingType = segment.TracingTypeId.HasValue ? _tracingTypeService.GetAll().Result.Single(m => m.Id == segment.TracingTypeId) : null;
                segment.TracingDesignNumberOfTracers = segment.TracingDesignNumberOfTracersId.HasValue
                                                     ? _tracingDesignNumberOfTracersService.GetAll().Result.Single(m => m.Id == segment.TracingDesignNumberOfTracersId)
                                                     : null;
                segment.InsulationMaterial = segment.InsulationMaterialId.HasValue ? _insulationMaterialService.GetAll().Result.Single(m => m.Id == segment.InsulationMaterialId) : null;
                segment.PaintSystem = segment.PaintSystemId.HasValue ? _paintSystemService.GetAll().Result.Single(m => m.Id == segment.PaintSystemId) : null;

                if (!segments.Contains(segment))
                    segments.Add(segment);
            }

            // remove stale segments
            var toRemove = rev.LineRevisionSegments.Where(s => s.ModifiedOn != now).ToList();
            foreach (var s in toRemove) rev.LineRevisionSegments.Remove(s);
        }

        private static Guid? GetCacheValue(Dictionary<string, Dictionary<string, Guid>> cache, string table, string key)
        {
            if (cache != null && cache.ContainsKey(table) && cache[table].ContainsKey(key))
                return cache[table][key];
            return null;
        }

        private static string GetCacheValue(Dictionary<string, Dictionary<string, Guid>> cache, string table, Guid id)
        {
            if (cache != null && cache.ContainsKey(table))
            {
                var kv = cache[table].FirstOrDefault(p => p.Value == id);
                return kv.Key;
            }
            return string.Empty;
        }

        private Guid GetLineStatus(LineRevision item, Dictionary<string, Dictionary<string, Guid>> cache, FlatLine flat)
        {
            string status;
            if (flat.Deleted == "Y") status = "Deleted";
            else if (!string.IsNullOrWhiteSpace(flat.ReservedBy)) status = "Reserved";
            else if (!string.IsNullOrWhiteSpace(flat.AsBuilt)) status = flat.AsBuilt;
            else if (item.LineListRevision?.LineListStatus?.CorrespondingLineStatus != null
                  && !item.LineListRevision.LineListStatus.Name.ToUpperInvariant().Equals("AS BUILT"))
                status = item.LineListRevision.LineListStatus.CorrespondingLineStatus.Name;
            else status = "IFC";

            if (cache != null && cache.ContainsKey("LineStatus") && cache["LineStatus"].ContainsKey(status.ToUpperInvariant()))
                return cache["LineStatus"][status.ToUpperInvariant()];

            // fallback: fetch actual entity
            var all = _lineStatusService.GetAll().Result;
            return all.FirstOrDefault(s => s.Name.Equals(status, StringComparison.OrdinalIgnoreCase))?.Id
                   ?? all.First(s => s.Name == "IFC").Id;
        }
    }
}
