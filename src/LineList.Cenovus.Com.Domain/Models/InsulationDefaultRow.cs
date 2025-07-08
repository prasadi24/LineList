using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Table("InsulationDefaultRow")]
    public class InsulationDefaultRow : LLTable
    {
        [Required]
        public Guid InsulationDefaultId { get; set; }  // Foreign Key to InsulationDefault

        public Guid? SizeNpsId { get; set; }  // Foreign Key to SizeNps (nullable)

        // Foreign Key Relationships
        [ForeignKey("InsulationDefaultId")]
        public virtual InsulationDefault InsulationDefault { get; set; }

        [ForeignKey("SizeNpsId")]
        public virtual SizeNps? SizeNps { get; set; }

        // 🔥 **Missing Navigation Property: Add the Collection**
        public virtual ICollection<InsulationDefaultDetail>? InsulationDefaultDetails { get; set; }
    }
}