using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("CorrosionAllowance")]
    public class CorrosionAllowance : LLLookupTableNoDescription
    {
        public override string ToString()
        {
            return Name;
        }
    }
}