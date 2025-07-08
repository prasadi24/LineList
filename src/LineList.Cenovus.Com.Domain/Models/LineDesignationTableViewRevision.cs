using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.Domain.Models
{
    public partial class LineDesignationTableViewRevision
    {
        public Guid LineListRevisionId { get; set; }

        public Guid LineListId { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentRevision { get; set; }
        public string Description { get; set; }
        public string LeadDiscEngineer { get; set; }
        public string ProjectEngineer { get; set; }
        public DateTime? IssuedOn { get; set; }
        public string ApprovedByLead { get; set; }
        public string ApprovedByProject { get; set; }
        public string PreparedBy { get; set; }
        public string ReviewedBy { get; set; }
        public string PreparedByProcess { get; set; }
        public string PreparedByMechanical { get; set; }
        public string ReviewByProcess { get; set; }
        public string ReviewedByMechanical { get; set; }
        public int DocumentRevisionSort { get; set; }
        public bool IsHardRevision { get; set; }
    }
}