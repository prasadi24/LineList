using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ImportFacility")]
    public class ImportFacility : Entity
    {
        public virtual Import Import { get; set; }
        public Guid ImportId { get; set; }

        public Guid FacilityId { get; set; }
        public virtual Facility Facility { get; set; }

        public int BeforeCount { get; set; }
        public int AfterCount { get; set; }
    }
}