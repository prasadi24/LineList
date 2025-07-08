using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ScheduleDefault
{
    public class ScheduleDefaultAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public int RevisionNumber { get; set; } = 1;

       
        public DateTime? RevisionDate { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        public int? SortOrder { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid PipeSpecificationId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid ScheduleId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid SizeNpsId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid SpecificationId { get; set; }
    }
}