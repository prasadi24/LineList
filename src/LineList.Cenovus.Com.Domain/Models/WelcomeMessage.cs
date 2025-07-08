using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("WelcomeMessage")]
    public partial class WelcomeMessage : Entity
    {
        [Required]
        public string Description { get; set; }

        public string? Notes { get; set; }

        public string? Message2 { get; set; }

        public string? Message3 { get; set; }

        [Required]
        [StringLength(50)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        [Required]
        [StringLength(50)]
        public string ModifiedBy { get; set; }

        [Required]
        public DateTime ModifiedOn { get; set; }
    }
}