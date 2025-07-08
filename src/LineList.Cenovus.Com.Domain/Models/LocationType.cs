using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("LocationType")]
    public class LocationType : LLLookupTableNoDescription
    {
        public string? Notes { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}