using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.UserPreference
{
    public class UserPreferenceAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(255, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string UserName { get; set; }

        [StringLength(255, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? FullName { get; set; }

        [StringLength(255, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}