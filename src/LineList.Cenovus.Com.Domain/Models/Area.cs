using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("Area")]
    public class Area : LLLookupTable
    {
        public Guid LocationId { get; set; }
        public Guid SpecificationId { get; set; }

        public virtual Location Location { get; set; }
        public virtual Specification Specification { get; set; }
    }
}