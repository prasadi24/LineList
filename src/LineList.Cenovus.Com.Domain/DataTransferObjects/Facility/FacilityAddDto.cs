using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Facility
{
    public class FacilityAddDto
    {
        [Required(ErrorMessage = "This field is required.")]       
        [StringLength(20, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(60, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Description { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Value must be 0 or a positive number.")]
        public int SortOrder { get; set; }

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
    }
}