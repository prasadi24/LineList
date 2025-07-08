using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration
{
    public class NotesConfigurationEditDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid FacilityId { get; set; }

        [Required]
        public Guid SpecificationId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(255)]
        public string FileName { get; set; }

        public byte[]? FileData { get; set; }

        [NotMapped] // Optional: prevents EF from mapping this if it's an EF entity
        public string FileSizeDisplay =>
            FileData == null || FileData.Length == 0
            ? "0 MB"
            : string.Format("{0:0.##} MB", FileData.Length / (1024.0 * 1024.0));

        [Required]
        [StringLength(100)]
        public string UploadedBy { get; set; }

        [Required]
        public DateTime UploadedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}
