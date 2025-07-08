using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.LineRevision
{
    public class LineRevisionAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool IsCheckedOut { get; set; }

        public bool IsJacketed { get; set; } = false;

        [Required(ErrorMessage = "This field is required.")]
        public int RevisionSort { get; set; } = 0;

        public int? ValidationState { get; set; } = 0;

        [StringLength(100)]
        public string? ModularId { get; set; }

        public Guid? AreaId { get; set; }

        public Guid? CorrosionAllowancePipeId { get; set; }

        public Guid? CorrosionAllowanceAnnulusId { get; set; }

        public Guid? InternalCoatingLinerId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid LineId { get; set; }

        public Guid? LineListRevisionId { get; set; }

        public Guid? LineStatusId { get; set; }

        public Guid? NdeCategoryPipeId { get; set; }

        public Guid? NdeCategoryAnnulusId { get; set; }

        public Guid? PipeSpecificationId { get; set; }

        public Guid? PostWeldHeatTreatmentId { get; set; }

        public Guid? SchedulePipeId { get; set; }

        public Guid? ScheduleAnnulusId { get; set; }

        public Guid? SizeNpsPipeId { get; set; }

        public Guid? SizeNpsAnnulusId { get; set; }

        public Guid? SpecificationId { get; set; }

        public Guid? StressAnalysisId { get; set; }

        public Guid? TestMediumPipeId { get; set; }

        public Guid? TestMediumAnnulusId { get; set; }

        public Guid? XrayPipeId { get; set; }

        public Guid? XrayAnnulusId { get; set; }
    }
}