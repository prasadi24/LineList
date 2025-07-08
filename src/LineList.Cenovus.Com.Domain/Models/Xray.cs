using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("Xray")]
    public class Xray : LLLookupTable
    {
        public override string ToString()
        {
            return Name;
        }
    }
}