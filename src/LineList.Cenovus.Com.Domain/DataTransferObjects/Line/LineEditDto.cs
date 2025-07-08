using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Line
{
    public class LineEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(5, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string SequenceNumber { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int ChildNumber { get; set; }

        [StringLength(100, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? ModularId { get; set; }

        public Guid? CommodityId { get; set; }

        public Guid? LocationId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string ModifiedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}