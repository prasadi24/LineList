using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefault
{
    public class InsulationDefaultResultDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Sort")]
        public int SortOrder { get; set; }

        public string? Description { get; set; }

        public string? SpecificationRevision { get; set; }

        public string? Name { get; set; }

        [Display(Name = "Link")]
        public string? LinkToDocument { get; set; }

        public string? Notes { get; set; }

        public Guid? InsulationMaterialId { get; set; }

        public Guid? InsulationTypeId { get; set; }

        public Guid? TracingTypeId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string InsulationMaterialName { get; set; }
        public string InsulationMaterialDescription { get; set; }
        public string InsulationTypeName { get; set; }
        public string InsulationTypeDescription { get; set; }
        public string TracingTypeName { get; set; }
        public string TracingTypeDescription { get; set; }
    }
}