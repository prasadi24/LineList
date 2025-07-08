using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("Commodity")]
    public class Commodity : LLLookupTable
    {
        public Guid SpecificationId { get; set; }

        public virtual Specification Specification { get; set; }
    }
}