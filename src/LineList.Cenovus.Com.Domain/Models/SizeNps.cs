using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("SizeNps")]
    public class SizeNps : LLLookupTableNoDescription
    {
        public string DecimalValue { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}