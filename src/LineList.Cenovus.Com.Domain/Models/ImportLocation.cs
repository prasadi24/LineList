using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ImportLocation")]
    public class ImportLocation : Entity
    {
        public Guid LocationId { get; set; }
        public virtual Location Location { get; set; }

        public int BeforeCount { get; set; }
        public int AfterCount { get; set; }

        public virtual Import Import { get; set; }
        public Guid ImportId { get; set; }
    }
}