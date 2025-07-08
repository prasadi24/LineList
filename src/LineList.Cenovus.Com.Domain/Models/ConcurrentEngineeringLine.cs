using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ConcurrentEngineeringLine")]
    public class ConcurrentEngineeringLine : Entity
    {
        public Guid LineId { get; set; }
        public string LineNumber { get; set; }
        public Guid LineListRevisionId { get; set; }
        public Guid EpProjectId { get; set; }

        public Guid FacilityId { get; set; }
        public int ChildNumber { get; set; }
        public string Location { get; set; }
        public string Commodity { get; set; }
        public string SequenceNumber { get; set; }
        public string DocumentNumber { get; set; }
        public string LineListStatus { get; set; }
        public string DocumentRevision { get; set; }
        public string EP { get; set; }
        public string LineRevision { get; set; }
        public string LineStatus { get; set; }
        public bool IsActive { get; set; }
        public string IsActiveText { get; set; }
        public bool IsDraft { get; set; }
        public string ParentChild { get; set; }
        public Guid LocationId { get; set; }
        public Guid CommodityId { get; set; }
        public Guid LineListId { get; set; }

        public string ShortLineNumber { get; set; }
        public string Specification { get; set; }
        public int AsBuiltCount { get; set; }

        public DateTime? IssuedOn { get; set; }
    }
}