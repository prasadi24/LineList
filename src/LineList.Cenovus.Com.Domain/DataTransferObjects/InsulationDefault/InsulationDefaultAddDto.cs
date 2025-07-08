using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.InsulationDefault
{
    public class InsulationDefaultAddDto
    {
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "This field is required.")]
        public int SortOrder { get; set; } = 0;

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(60, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? SpecificationRevision { get; set; }

        [Required(ErrorMessage = "This field is required.")]

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? SpecificationRevisionDate { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string? Name { get; set; }
        public string? SpecificationName { get; set; }

        [StringLength(255, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? LinkToDocument { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid? InsulationMaterialId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid? InsulationTypeId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid? TracingTypeId { get; set; }
        public Guid? SpecificationId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));
    }
}