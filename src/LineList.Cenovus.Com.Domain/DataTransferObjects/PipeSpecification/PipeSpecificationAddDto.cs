using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.PipeSpecification
{
    public class PipeSpecificationAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(20, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string Name { get; set; }

        [StringLength(60, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string Description { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool IsActive { get; set; }

        public string? RevisionNumber { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime? RevisionDate { get; set; }

        public Guid? CorrosionAllowanceId { get; set; }

        public Guid? NdeCategoryId { get; set; }

        public Guid? PostWeldHeatTreatmentId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid? SpecificationId { get; set; }

        public Guid? XrayId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}