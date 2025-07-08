using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
	[Serializable]
	[Table("LineRevision")]
	public class LineRevision : LLTable
	{
		[Required]
		[ForeignKey("Line")]
		public Guid LineId { get; set; }

		public virtual Line Line { get; set; }

		[ForeignKey("LineListRevision")]
		public Guid? LineListRevisionId { get; set; }

		public virtual LineListRevision LineListRevision { get; set; }
		public virtual ICollection<LineRevisionOperatingMode> LineRevisionOperatingModes { get; set; }

		public virtual ICollection<LineRevisionSegment> LineRevisionSegments { get; set; }

		public string LineNumber { get; set; }

		public int ValidationState { get; set; }

		public bool IsActive { get; set; }

		public bool IsJacketed { get; set; }

		public Guid? AreaId { get; set; }
		public virtual Area Area { get; set; }

		public bool IsCheckedOut { get; set; }
		public string? CheckedOutBy { get; set; }
		public DateTime? CheckedOutOn { get; set; }

		public Guid? CorrosionAllowancePipeId { get; set; }
		public virtual CorrosionAllowance CorrosionAllowancePipe { get; set; }

		public Guid? CorrosionAllowanceAnnulusId { get; set; }
		public virtual CorrosionAllowance CorrosionAllowanceAnnulus { get; set; }

		public string? DesignPressurePipe { get; set; }
		public string? DesignPressureAnnulus { get; set; }

		public string? DesignTemperatureMaximumPipe { get; set; }
		public string? DesignTemperatureMinimumPipe { get; set; }

		public string? DesignTemperatureMaximumAnnulus { get; set; }
		public string? DesignTemperatureMinimumAnnulus { get; set; }

		public string? ExpansionTemperature { get; set; }

		public Guid? InternalCoatingLinerId { get; set; }
		public virtual InternalCoatingLiner InternalCoatingLiner { get; set; }

		public string? MinimumDesignMetalTemperature { get; set; }
		public bool RequiresMinimumInformation { get; set; }

		public Guid? NdeCategoryPipeId { get; set; }
		public virtual NdeCategory NdeCategoryPipe { get; set; }

		public Guid? NdeCategoryAnnulusId { get; set; }
		public virtual NdeCategory NdeCategoryAnnulus { get; set; }

		public Guid? OtherInspectionId { get; set; }

		public string? OriginatingPID { get; set; }

		public string? ModularId { get; set; }

		public Guid? PipeSpecificationId { get; set; }
		public virtual PipeSpecification PipeSpecification { get; set; }

		//SER 435.  Changes by Armando Chaves.
		//public Guid? PressureProtectionId { get; set; }
		//public virtual PressureProtection PressureProtection { get; set; }

		public bool IsReferenceLine { get; set; }
		public string? Revision { get; set; }
		public int RevisionSort { get; set; }

		public Guid? SchedulePipeId { get; set; }
		public virtual Schedule SchedulePipe { get; set; }

		public Guid? ScheduleAnnulusId { get; set; }
		public virtual Schedule ScheduleAnnulus { get; set; }

		public Guid? SizeNpsPipeId { get; set; }
		public virtual SizeNps SizeNpsPipe { get; set; }

		public Guid? SizeNpsAnnulusId { get; set; }
		public virtual SizeNps SizeNpsAnnulus { get; set; }

		public Guid? SpecificationId { get; set; }
		public virtual Specification Specification { get; set; }

		public Guid? LineStatusId { get; set; }
		public virtual LineStatus LineStatus { get; set; }

		public Guid? StressAnalysisId { get; set; }
		public virtual StressAnalysis StressAnalysis { get; set; }

		public Guid? PostWeldHeatTreatmentId { get; set; }
		public virtual PostWeldHeatTreatment PostWeldHeatTreatment { get; set; }

		public string? TestPressurePipe { get; set; }
		public string? TestPressureAnnulus { get; set; }

		public Guid? TestMediumPipeId { get; set; }
		public virtual TestMedium TestMediumPipe { get; set; }

		public Guid? TestMediumAnnulusId { get; set; }
		public virtual TestMedium TestMediumAnnulus { get; set; }

		public string? UpsetPressurePipe { get; set; }
		public string? UpsetPressureAnnulus { get; set; }

		public string? UpsetTemperaturePipe { get; set; }
		public string? UpsetTemperatureAnnulus { get; set; }

		public decimal? WallThicknessPipe { get; set; }
		public decimal? WallThicknessAnnulus { get; set; }

		public Guid? XrayPipeId { get; set; }
		public virtual Xray XrayPipe { get; set; }

		public Guid? XrayAnnulusId { get; set; }
		public virtual Xray XrayAnnulus { get; set; }
	}
}