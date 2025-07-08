using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("Specification")]
    public class Specification : LLLookupTable
    {
        public string? DocumentLink { get; set; }

        public virtual ICollection<Area> Areas { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}