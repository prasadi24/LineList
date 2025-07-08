using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Import
{
    public class ImportEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string OriginalFileName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string FacilityName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Path { get; set; }

        public int? ValidationCount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string ModifiedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}