using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefault
{
    public class InsulationDefaultEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int SortOrder { get; set; }

        [StringLength(60, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? SpecificationRevision { get; set; }

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Name { get; set; }
        public string? SpecificationName { get; set; }

        [StringLength(255, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? LinkToDocument { get; set; }

        public string? Notes { get; set; }
        public DateTime? SpecificationRevisionDate { get; set; }

        public Guid? InsulationMaterialId { get; set; }

        public Guid? InsulationTypeId { get; set; }

        public Guid? TracingTypeId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string ModifiedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}