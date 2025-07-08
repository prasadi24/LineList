using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefaultRow
{
    public class InsulationDefaultRowAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid InsulationDefaultId { get; set; }

        public Guid? SizeNpsId { get; set; }

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