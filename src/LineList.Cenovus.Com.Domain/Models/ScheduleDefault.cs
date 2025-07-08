using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ScheduleDefault")]
    public class ScheduleDefault : LLTable
    {

        public bool IsActive { get; set; }

        public string? Notes { get; set; }
        public int? SortOrder { get; set; }

        public int RevisionNumber { get; set; }
        public DateTime RevisionDate { get; set; }

        public Guid? ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        public Guid PipeSpecificationId { get; set; }

        public virtual PipeSpecification PipeSpecification { get; set; }

        public Guid SizeNpsId { get; set; }
        public virtual SizeNps SizeNps { get; set; }
    }
}