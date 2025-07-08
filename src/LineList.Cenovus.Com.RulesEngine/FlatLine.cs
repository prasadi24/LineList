namespace LineList.Cenovus.Com.RulesEngine
{
    [Serializable]
    public class FlatLine
    {
        public Guid Id;
        public Guid? LineListRevisionId;
        public bool IsJacketed;
        public bool IsCsa;
        public bool RequiresValidation;
        public int RowNumber;
        public int ChildNumber;

        //SER798
        public string RownumberNew;

        public string FacilityID; //IRQ0335326. Gabriel Zuñiga
        public string Checked;
        public string Duplicate;
        public string Deleted;
        public string ReservedBy;
        public string AltOpMode;
        public string ParentChild;
        public string Area;
        public string Specification;
        public string Location;
        public string Commodity;
        public string ClassServMaterial;
        public string SizeNps;
        public string SequenceNumber;
        public string InsulationType;
        public string TraceDesignType;
        public string TraceDesignHoldTemp;
        public string TracingDesignNumTracers;
        public string InsulationMaterial;
        public string LineFrom;
        public string LineTo;
        public string OriginatingPID;
        public string Schedule;
        public string InsulationThickness;
        public string FluidPhase;
        public string OperatingPressure;
        public string OperatingTemperature;
        public string OpMode;
        public string DesignPressure;
        public string DesignMaxTemperature;
        public string DesignMinTemperature;
        public string TestPressure;
        public string TestMedium;
        public string ExpTemperature;
        public string UpsetPressure;
        public string UpsetTemperature;
        public string MdmtTemperature;
        public string CorrosionAllowance;
        public string Xray;
        public string NdeCategory;
        public string PostWeldHeatTreatment;
        public string StressAnalysis;
        public string PaintSystem;
        public string InternalCoatingLiner;
        public string Code;
        public string AbsaRegistration;
        public string PressureProtection;
        public string LineStatus;
        public string Fluid;
        public string CsaClassLocation;
        public string CsaHvpLvp;
        public string PipeMaterialSpecification;
        public string HoopStressLevel;
        public string SourService;
        public string Notes;
        public string LineRevision;
        public string DocumentNumber;
        public string DocumentRevision;
        public string IssuedOn;
        public string EpCompany;
        public string EpProject;
        public string WallThickness;
        public string AsBuilt;
        public string SegmentType;
        public string SegmentNumber;
        public string ModularId;

        public FlatSourceEnum Source { get; set; }

        public string annuCorrosionAllowance;
        public string annuDesignMaxTemperature;
        public string annuDesignMinTemperature;
        public string annuDesignPressure;
        public string annuNdeCategory;
        public string annuOperatingPressure;
        public string annuOperatingTemperature;
        public string annuSchedule;
        public string annuSizeNps;
        public string annuTestPressure;
        public string annuTestMedium;
        public string annuUpsetPressure;
        public string annuUpsetTemperature;
        public string annuWallThickness;
        public string annuXray;

        public string pipeCorrosionAllowance;
        public string pipeDesignMaxTemperature;
        public string pipeDesignMinTemperature;
        public string pipeDesignPressure;
        public string pipeNdeCategory;
        public string pipeOperatingPressure;
        public string pipeOperatingTemperature;
        public string pipeSchedule;
        public string pipeSizeNps;
        public string pipeTestPressure;
        public string pipeTestMedium;
        public string pipeUpsetPressure;
        public string pipeUpsetTemperature;
        public string pipeWallThickness;
        public string pipeXray;

        public Guid JacketId;

        //these are only here so the reports can be bound to them.
        public Guid rpt_Id
        { get { return this.Id; } }

        public Guid? rpt_LineListRevisionId
        { get { return this.Id; } }
        public bool rpt_IsJacketed
        { get { return this.IsJacketed; } }
        public bool rpt_IsCsa
        { get { return this.IsCsa; } }
        public bool rpt_RequiresValidation
        { get { return this.RequiresValidation; } }
        public int rpt_RowNumber
        { get { return this.RowNumber; } }
        public string rpt_Checked
        { get { return this.Checked; } }
        public string rpt_Duplicate
        { get { return this.Duplicate; } }
        public string rpt_Deleted
        { get { return this.Deleted; } }
        public string rpt_ReservedBy
        { get { return this.ReservedBy; } }
        public string rpt_AltOpMode
        { get { return this.AltOpMode; } }
        public string rpt_ParentChild
        { get { return this.ParentChild; } }
        public int rpt_ChildNumber
        { get { return this.ChildNumber; } }
        public string rpt_Area
        { get { return this.Area; } }
        public string rpt_Specification
        { get { return this.Specification; } }
        public string rpt_Location
        { get { return this.Location; } }
        public string rpt_Commodity
        { get { return this.Commodity; } }
        public string rpt_ClassServMaterial
        { get { return this.ClassServMaterial; } }
        public string rpt_SizeNps
        { get { return this.SizeNps; } }
        public string rpt_SequenceNumber
        { get { return this.SequenceNumber; } }

        public string rpt_InsulationType
        { get { return this.InsulationType; } }
        public string rpt_TraceDesignType
        { get { return this.TraceDesignType; } }
        public string rpt_TraceDesignHoldTemp
        { get { return this.TraceDesignHoldTemp; } }
        public string rpt_TracingDesignNumTracers
        { get { return this.TracingDesignNumTracers; } }
        public string rpt_InsulationMaterial
        { get { return this.InsulationMaterial; } }
        public string rpt_LineFrom
        { get { return this.LineFrom; } }
        public string rpt_LineTo
        { get { return this.LineTo; } }
        public string rpt_OriginatingPID
        { get { return this.OriginatingPID; } }
        public string rpt_Schedule
        { get { return this.Schedule; } }
        public string rpt_InsulationThickness
        { get { return this.InsulationThickness; } }
        public string rpt_FluidPhase
        { get { return this.FluidPhase; } }
        public string rpt_OperatingPressure
        { get { return this.OperatingPressure; } }
        public string rpt_OperatingTemperature
        { get { return this.OperatingTemperature; } }
        public string rpt_OpMode
        { get { return this.OpMode; } }
        public string rpt_DesignPressure
        { get { return this.DesignPressure; } }
        public string rpt_DesignMaxTemperature
        { get { return this.DesignMaxTemperature; } }
        public string rpt_DesignMinTemperature
        { get { return this.DesignMinTemperature; } }
        public string rpt_TestPressure
        { get { return this.TestPressure; } }
        public string rpt_TestMedium
        { get { return this.TestMedium; } }
        public string rpt_ExpTemperature
        { get { return this.ExpTemperature; } }
        public string rpt_UpsetPressure
        { get { return this.UpsetPressure; } }
        public string rpt_UpsetTemperature
        { get { return this.UpsetTemperature; } }
        public string rpt_MdmtTemperature
        { get { return this.MdmtTemperature; } }
        public string rpt_CorrosionAllowance
        { get { return this.CorrosionAllowance; } }
        public string rpt_Xray
        { get { return this.Xray; } }
        public string rpt_NdeCategory
        { get { return this.NdeCategory; } }
        public string rpt_PostWeldHeatTreatment
        { get { return this.PostWeldHeatTreatment; } }
        public string rpt_StressAnalysis
        { get { return this.StressAnalysis; } }
        public string rpt_PaintSystem
        { get { return this.PaintSystem; } }
        public string rpt_InternalCoatingLiner
        { get { return this.InternalCoatingLiner; } }
        public string rpt_Code
        { get { return this.Code; } }
        public string rpt_AbsaRegistration
        { get { return this.AbsaRegistration; } }
        public string rpt_PressureProtection
        { get { return this.PressureProtection; } }
        public string rpt_LineStatus
        { get { return this.LineStatus; } }
        public string rpt_Fluid
        { get { return this.Fluid; } }
        public string rpt_CsaClassLocation
        { get { return this.CsaClassLocation; } }
        public string rpt_CsaHvpLvp
        { get { return this.CsaHvpLvp; } }
        public string rpt_PipeMaterialSpecification
        { get { return this.PipeMaterialSpecification; } }
        public string rpt_HoopStressLevel
        { get { return this.HoopStressLevel; } }
        public string rpt_SourService
        { get { return this.SourService; } }
        public string rpt_Notes
        { get { return this.Notes; } }
        public string rpt_LineRevision
        { get { return this.LineRevision; } }

        public string rpt_DocumentNumber
        { get { return this.DocumentNumber; } }
        public string rpt_DocumentRevision
        { get { return this.DocumentRevision; } }
        public string rpt_IssuedOn
        { get { return this.IssuedOn; } }
        public string rpt_EpCompany
        { get { return this.EpCompany; } }
        public string rpt_EpProject
        { get { return this.EpProject; } }
        public string rpt_WallThickness
        { get { return this.WallThickness; } }
        public string rpt_AsBuilt
        { get { return this.AsBuilt; } }
        public string rpt_SegmentType
        { get { return this.SegmentType; } }
        public string rpt_SegmentNumber
        { get { return this.SegmentNumber; } }

        public string rpt_ModularId
        { get { return this.ModularId; } }

        public string rpt_SchWallThk
        {
            get
            {
                if (this.IsJacketed)
                {
                    var pipe = string.IsNullOrWhiteSpace(this.pipeSchedule) ? string.IsNullOrWhiteSpace(this.pipeWallThickness) ? string.Empty : this.pipeWallThickness : this.pipeSchedule;
                    var annulus = string.IsNullOrWhiteSpace(this.annuSchedule) ? string.IsNullOrWhiteSpace(this.annuWallThickness) ? string.Empty : " (JK:" + this.annuWallThickness + ")" : " (JK:" + this.annuSchedule + ")";
                    return (pipe ?? string.Empty) + (annulus ?? string.Empty);
                }
                else
                    return string.IsNullOrWhiteSpace(this.Schedule) ? this.WallThickness : this.Schedule;
            }
        }
    }
}