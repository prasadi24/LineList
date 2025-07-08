using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("NotesConfiguration")]
    public partial class NotesConfiguration : Entity
    {
        [Required]
        public Guid FacilityId { get; set; }

        [Required]
        public Guid SpecificationId { get; set; }

        [Required]
        [StringLength(255)]
        public string FileName { get; set; }

        [Required]
        public byte[]? FileData { get; set; }

        [Required]
        [StringLength(100)]
        public string UploadedBy { get; set; }

        [Required]
        public DateTime UploadedOn { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
    }
}
