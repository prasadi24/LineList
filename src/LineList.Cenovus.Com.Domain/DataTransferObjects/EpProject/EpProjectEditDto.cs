using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProject
{
    public class EpProjectEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(20, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required.")]

        [StringLength(60, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Description { get; set; }

        public Guid? CenovusProjectId { get; set; }
        public Guid FacilityId { get; set; }
        public Guid ProjectTypeId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpCompanyId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int SortOrder { get; set; }

        public string? Notes { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string ModifiedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        public Guid? CopyInsulationTableDefaultsEpProjectId { get; set; }
    }
}