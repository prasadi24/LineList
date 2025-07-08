using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpCompanyAlpha
{
    public class EpCompanyAlphaAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(2, ErrorMessage = "The field {0} must be exactly {1} characters long")]
        public string Alpha { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpCompanyId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid FacilityId { get; set; }

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