using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.TestPressure
{
    public class TestPressureAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(150, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string Name { get; set; }

        [StringLength(150, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [StringLength(150, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Notes { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(150, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}