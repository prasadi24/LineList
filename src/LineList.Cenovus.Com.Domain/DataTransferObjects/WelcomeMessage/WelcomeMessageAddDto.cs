using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.WelcomeMessage
{
    public class WelcomeMessageAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public string Description { get; set; }

        public string? Notes { get; set; }

        public string? Message2 { get; set; }

        public string? Message3 { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}