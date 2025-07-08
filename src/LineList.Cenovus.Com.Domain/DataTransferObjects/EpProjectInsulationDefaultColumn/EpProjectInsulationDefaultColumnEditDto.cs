using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultColumn
{
    public class EpProjectInsulationDefaultColumnEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }
        [Display(Name = "Operating Minimum")]
        public double? MinOperatingTemperature { get; set; }
        [Display(Name = "Operating Maximum")]
        public double? MaxOperatingTemperature { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string ModifiedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpProjectInsulationDefaultId { get; set; }
    }
}