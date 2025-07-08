using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("Location")]
    public class Location : LLLookupTable
    {
        public Guid FacilityId { get; set; }
        public Guid LocationTypeId { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual LocationType LocationType { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}