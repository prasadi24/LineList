using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ConcurrentEngineeringLine
{
    public class ConcurrentEngineeringLineResultDto
    {
        public Guid Id { get; set; }
        public Guid LineId { get; set; }
        [Display(Name = "Line Number")]
        public string LineNumber { get; set; }

        [Display(Name = "Line List Revision")]
        public Guid LineListRevisionId { get; set; }

        [Display(Name = "EP Project")]
        public Guid EpProjectId { get; set; }

        [Display(Name = "Facility")]
        public Guid FacilityId { get; set; }

        [Display(Name = "Child Number")]
        public int ChildNumber { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }

        [Display(Name = "Commodity")]
        public string Commodity { get; set; }

        [Display(Name = "Sequence Number")]
        public string SequenceNumber { get; set; }

        [Display(Name = "Document")]
        public string DocumentNumber { get; set; }

        [Display(Name = "Line List Status")]
        public string LineListStatus { get; set; }

        [Display(Name = "Document Revision")]
        public string DocumentRevision { get; set; }

        [Display(Name = "EP")]
        public string EP { get; set; }

        [Display(Name = "Line Revision")]
        public string LineRevision { get; set; }

        [Display(Name = "Line Status")]
        public string LineStatus { get; set; }

        [Display(Name = "Is Active Text")]
        public string IsActiveText { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Is Draft")]
        public bool IsDraft { get; set; }

        [Display(Name = "Parent/Child")]
        public string ParentChild { get; set; }

        [Display(Name = "Location ID")]
        public Guid LocationId { get; set; }

        [Display(Name = "Commodity ID")]
        public Guid CommodityId { get; set; }

        [Display(Name = "Line List ID")]
        public Guid LineListId { get; set; }

        [Display(Name = "Short Line Number")]
        public string ShortLineNumber { get; set; }

        [Display(Name = "Spec")]
        public string Specification { get; set; }

        [Display(Name = "As Built Count")]
        public int AsBuiltCount { get; set; }

        [Display(Name = "Issued On")]
        public DateTime? IssuedOn { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Modified By")]
        public string ModifiedBy { get; set; }

        [Display(Name = "Modified On")]
        public DateTime ModifiedOn { get; set; }

        // Additional properties
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }

        [Display(Name = "Commodity Name")]
        public string CommodityName { get; set; }

        [Display(Name = "Line List Actions")]
        public string LineListActions { get; set; }

        [Display(Name = "Line Actions")]
        public string? LineActions { get; set; }

        [Display(Name = "LDT Issue Date")]
        public DateTime? LdtIssueDate { get; set; }

        // Added properties to resolve errors
        [Display(Name = "Line List Status Is Hard Revision")]
        public bool LineListStatusIsHardRevision { get; set; }

        [Display(Name = "Line List Status Is Issued Of ID")]
        public bool LineListStatusIsIssuedOfId { get; set; }

        [Display(Name = "EP Company ID")]
        public Guid EpCompanyId { get; set; }

    }
}
