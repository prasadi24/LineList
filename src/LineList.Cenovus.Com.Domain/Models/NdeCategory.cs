using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("NdeCategory")]
    public class NdeCategory : LLLookupTableNoDescription
    {
        public Guid? XrayId { get; set; }

        public virtual Xray Xray { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}