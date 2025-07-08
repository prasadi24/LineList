using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Table("LineDesignationTableViewHeader")]
    public partial class LineDesignationTableViewHeader : Entity
    {
        public string Description { get; set; }

        public bool IsSimpleRevisionBlock { get; set; }
        public bool IsLocked { get; set; }
        public bool IsDraft { get; set; }
        public bool IsIssued { get; set; }

        public string DocumentRevision { get; set; }
        public string SpecDesc { get; set; }
        public string SpecName { get; set; }
        public string AreaDesc { get; set; }
        public string AreaName { get; set; }
        public string DocumentNumber { get; set; }
        public Nullable<Guid> LineListId { get; set; }
        public string EppDesc { get; set; }
        public string EppName { get; set; }
        public string EpcDesc { get; set; }
        public string EpcName { get; set; }
        public string CpDesc { get; set; }
        public string CpName { get; set; }
        public string FacDesc { get; set; }
        public string FacName { get; set; }
        public string LocDesc { get; set; }
        public string LocName { get; set; }

        public byte[] EpcLogo { get; set; }
    }
}