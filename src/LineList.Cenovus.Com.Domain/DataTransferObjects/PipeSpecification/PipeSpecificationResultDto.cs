using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.PipeSpecification
{
    public class PipeSpecificationResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Sort")]
        public int SortOrder { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string? RevisionNumber { get; set; }

        public DateTime? RevisionDate { get; set; }

        public Guid? CorrosionAllowanceId { get; set; }

        public Guid? NdeCategoryId { get; set; }

        public Guid? PostWeldHeatTreatmentId { get; set; }

        public Guid? SpecificationId { get; set; }

        public Guid? XrayId { get; set; }

        [Display(Name = "Specification")]
        public string SpecificationName { get; set; }

        [Display(Name = "Corrosion Allowance Default")]
        public string CorrosionAllowanceName { get; set; }

        [Display(Name = "NDE Category Default")]
        public string NdeCategoryName { get; set; }

        [Display(Name = "X-ray Default")]
        public string XrayName { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}