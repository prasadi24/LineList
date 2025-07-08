using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultDetail
{
    public class EpProjectInsulationDefaultDetailEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string ModifiedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpProjectInsulationDefaultColumnId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpProjectInsulationDefaultRowId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid InsulationThicknessId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid TracingDesignNumberOfTracersId { get; set; }
       
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}