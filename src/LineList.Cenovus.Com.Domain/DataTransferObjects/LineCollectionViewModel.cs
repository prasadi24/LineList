using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class LineCollectionViewModel:Entity
    {
        public List<Specification> Specifications { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<Area> Areas { get; set; } = new();
        public List<Commodity> Commodities { get; set; }
        public List<PipeSpecification> PipeSpecifications { get; set; } = new();
        public List<LineStatus> LineStatuses { get; set; } = new();
        public List<SizeNps> SizeNpsPipes { get; set; } = new();
        public List<LineRevisionOperatingMode> LineRevisionOperatingModes { get; set; } = new();

        public List<OperatingMode> OperatingModes { get; set; } = new();

        public List<FluidPhase> FluidPhases { get; set; } = new();
        public List<PressureProtection> PressureProtections { get; set; } = new();
        public List<Schedule> Schedules { get; set; } = new();
        public List<TestMedium> TestMediums { get; set; } = new();
        public List<CorrosionAllowance> CorrosionAllowances { get; set; } = new();
        public List<Xray> Xrays { get; set; } = new();
        public List<NdeCategory> NDECategories { get; set; } = new();
        public List<PostWeldHeatTreatment> PWHTOptions { get; set; } = new();
        public List<Fluid> Fluids { get; set; } = new();
        public List<CsaClassLocation> CsaClassLocations { get; set; } = new();
        public List<CsaHvpLvp> CsaHvpLvps { get; set; } = new();
        public List<StressAnalysis> StressAnalyses { get; set; } = new();
        public List<InternalCoatingLiner> InternalCoatingLiners { get; set; } = new();
        public List<Code> Codes { get; set; } = new();
        public List<SegmentType> SegmentTypes { get; set; } = new();
        public List<InsulationMaterial> InsulationMaterials { get; set; } = new();
        public List<InsulationType> InsulationTypes { get; set; } = new();
        public List<TracingType> TracingTypes { get; set; } = new();
        public List<TracingDesignNumberOfTracers> TracingDesignNumTracers { get; set; } = new();
        public List<InsulationThickness> InsulationThicknesses { get; set; } = new();
        public List<PaintSystem> PaintSystems { get; set; } = new();
        public List<LineRevisionSegment> LineRevisionSegment { get; set; } = new();

        //Line Collection
        public Guid? LineListRevisionId { get; set; }
        public Guid? ReservedLineListRevisionId { get; set; }
        public Guid? LineId { get; set; }
        public string DocumentNumber { get; set; }
        public string? CheckedOutBy { get; set; }
        public string SpecificationName { get; set; }
        public string ParentChild { get; set; }
        public string AreaName { get; set; }
        public string LocationName { get; set; }
        public string CommodityName { get; set; }
        public string SizeNpsPipeName { get; set; }
        public string SequenceNumber { get; set; }
        public string LineStatusName { get; set; }
        public string Revision { get; set; }
        public int ValidationState { get; set; }
        public int ChildNumber { get; set; }
        public string PipeSpecificationName { get; set; }
        public string ModularId { get; set; }

        //public List<Guid> LineIds { get; set; }


        // NEW: flag used in the Index view to separate reference vs existing lines
        public bool IsReferenceLine { get; set; }


        //LineDetails
        public Guid AreaId { get; set; }
        public Guid PipeSpecificationId { get; set; }
        public Guid SizeNpsPipeId { get; set; }
        public Guid FluidPhaseId { get; set; }
        public Guid PressureProtectionId { get; set; }
        public Guid ScheduleId { get; set; }
        public Guid CorrosionAllowanceId { get; set; }
        public Guid StressAnalysisId { get; set; }
        public Guid XRayId { get; set; }
        public Guid InternalCoatingId { get; set; }
        public Guid NDECategoryId { get; set; }
        public Guid CodeId { get; set; }
        public Guid TestMediumId { get; set; }
        public Guid PWHTId { get; set; }
        public Guid FluidId { get; set; }
        public Guid CsaHvpLvpId { get; set; }
        public Guid CsaClassLocationId { get; set; }
        public Guid SegmentTypeId { get; set; }
        public Guid InsulationMaterialId { get; set; }
        public Guid InsulationTypeId { get; set; }
        public Guid TracingDesignId { get; set; }
        public Guid InsulationThicknessId { get; set; }
        public Guid NumTracerId { get; set; }
        public Guid PaintSystemId { get; set; }
        public Guid LineRevisionOperatingModeId { get; set; }

        public bool IsCsa { get; set; }

        public string LineRoutingFrom { get; set; }
        public string LineRoutingTo { get; set; }

        public string OperatingModeNumber { get; set; }
        public Guid? OperatingModeId { get; set; }
        public string OperatingModeIdDescription { get; set; }
        public string OriginatingPID { get; set; }
        public bool IsChild { get; set; }
        public string LineRevisionStatus { get; set; }
        public string OperatingPressurePipe { get; set; }
        public string OperatingTemp { get; set; }
        public string FluidPhaseName { get; set; }
        public string PressureProtectionName { get; set; }
        public string DesignPressure { get; set; }
        public string DesignTempMax { get; set; }
        public string DesignTempMin { get; set; }
        public string Notes { get; set; }
        public string ExpansionTemp { get; set; }
        public string UpsetPressure { get; set; }
        public string UpsetTemp { get; set; }
        public string ScheduleName { get; set; }
        public string WallThickness { get; set; }
        public string StressAnalysisName { get; set; }
        public string TestPressure { get; set; }
        public string TestMediumName { get; set; }
        public string MDMT { get; set; }
        public string CorrosionAllowanceName { get; set; }
        public string XRayName { get; set; }
        public string InternalCoatingName { get; set; }
        public string NDECategoryName { get; set; }
        public string CodeName { get; set; }
        public string PWHTName { get; set; }
        public string FluidName { get; set; }
        public string CsaClassLocationName { get; set; }
        public string CsaHvpLvpName { get; set; }
        public string PipeMaterialSpec { get; set; }
        public string HoopStressLevel { get; set; }
        public bool SourService { get; set; }

        public string IsAbsaRegistration { get; set; }
        
        public string SegmentTypeName { get; set; }
        public string InsulationMaterialName { get; set; }
        public string InsulationTypeName { get; set; }
        public string TracingDesignName { get; set; }
        public string InsulationThicknessName { get; set; }
        public string NumTracers { get; set; }
        public string TracingDesignHoldTemp { get; set; }
        public string PaintSystemName { get; set; }

        public string Jacket { get; set; }
        public Guid JacketSizeNpsPipeId { get; set; }
        public string JacketOperatingPressure { get; set; }
        public string JacketDesignPressure { get; set; }
        public string JacketUpsetPressure { get; set; }
        public string JacketOperatingTemp { get; set; }
        public string JacketDesignTempMin { get; set; }
        public string JacketUpsetTemp { get; set; }
        public string JacketDesignTempMax { get; set; }
        public Guid JacketScheduleId { get; set; }
        public Guid JacketCorrosionAllowanceId { get; set; }
        public string JacketWallThickness { get; set; }
        public Guid JacketXRayId { get; set; }
        public string JacketTestPressure { get; set; }
        public Guid JacketNDECategoryId { get; set; }
        public Guid JacketTestMediumId { get; set; }


        public bool IsActive { get; set; }
        public bool IsMinimumInformationCompliance { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }

    public class LineCheckIn_CheckOutRequest
    {
        public List<Guid> LineIds { get; set; }
        public Guid LineListRevisionId { get; set; }
    }

    public class PasteAttributesRequest
    {
        public Guid[] ToLineIds { get; set; }
    }


}