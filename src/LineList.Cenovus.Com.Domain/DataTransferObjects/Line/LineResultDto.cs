using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Line
{
    public class LineResultDto
    {
        public Guid Id { get; set; }

        public Guid LineId { get; set; }

        public string SequenceNumber { get; set; }

        public int ChildNumber { get; set; }

        public string? ModularId { get; set; }

        public Guid? CommodityId { get; set; }

        public Guid? LocationId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public string DocumentNumber { get; set; }

        public string AreaName { get; set; }
        public string CenovusProjectName { get; set; }
        public string Description { get; set; }
        public string DocumentRevision { get; set; }
        public string DocumentLineListRevision { get; set; }
        public int DocumentRevisionSort { get; set; }
        public string EpCompanyName { get; set; }
        public Guid EpProjectId { get; set; }
        public string EpProjectName { get; set; }
        public Guid FacilityId { get; set; }
        public string FacilityName { get; set; }
        public bool HasLines { get; set; }
        public Guid LineListStatusId { get; set; }
        public string LineListStatusName { get; set; }
        public string LineStatusName { get; set; }

        public string LocationName { get; set; }
        public string ProjectTypeName { get; set; }
        public DateTime? IssuedOn { get; set; }
        public string SpecificationName { get; set; }
        public string PipeSpecificationName { get; set; }
        public string CommoditiyName { get; set; }
        public string SizeNpsName { get; set; }
        public string ParentChild { get; set; }
        public bool IsDraft { get; set; }
        public bool IsHardRevision { get; set; }
        public bool IsIssued { get; set; }
        public bool IsHighestRev { get; set; }
        public bool HasActiveDrafts { get; set; }
        public bool HasEpCompanyAlpha { get; set; }
        public bool IsEpActive { get; set; }
        public bool NewHasEpCompanyAlpha { get; set; }
        public bool NewIsEpActive { get; set; }
        public string NewEpCompanyName { get; set; }


        public Guid? LineListRevisionId { get; set; }

        [Display(Name = "Line Revision")]
        public string LineRevision { get; set; }
    }
}