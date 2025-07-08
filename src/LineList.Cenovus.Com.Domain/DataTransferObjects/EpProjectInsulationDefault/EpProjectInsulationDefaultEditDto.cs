using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefault
{
    public class EpProjectInsulationDefaultEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(60, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? SpecificationRevision { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string? SpecificationName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime? SpecificationRevisionDate { get; set; }

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Name { get; set; }

        public string? Notes { get; set; }

        [StringLength(255, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? LinkToDocument { get; set; }

        public int SortOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpProjectId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid InsulationMaterialId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid InsulationTypeId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid TracingTypeId { get; set; }

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