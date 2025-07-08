using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("TracingType")]
    public class TracingType : LLLookupTable
    {
        public Guid SpecificationId { get; set; }
        public string? Temperature { get; set; }
        public bool IsJacketed { get; set; }
        public virtual Specification Specification { get; set; }

        [NotMapped]
        public string IsJacketedText
        { get { return this.IsJacketed ? "Yes" : "No"; } }
    }
}