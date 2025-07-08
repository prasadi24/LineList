using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration
{
    public class NotesConfigurationAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid FacilityId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid SpecificationId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(255)]
        public string FileName { get; set; }

        public byte[]? FileData { get; set; }

        [Required]
        [StringLength(100)]
        public string UploadedBy { get; set; }

        [Required]
        public DateTime UploadedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}
