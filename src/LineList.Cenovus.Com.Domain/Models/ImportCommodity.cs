using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ImportCommodity")]
    public class ImportCommodity : Entity
    {
        public virtual Import Import { get; set; }
        public Guid ImportId { get; set; }

        public Guid CommodityId { get; set; }
        public virtual Commodity Commodity { get; set; }

        public int BeforeCount { get; set; }
        public int AfterCount { get; set; }
    }
}