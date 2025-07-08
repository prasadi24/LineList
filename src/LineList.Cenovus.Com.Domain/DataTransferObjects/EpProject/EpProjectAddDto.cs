using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProject
{
    public class EpProjectAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(20, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(60, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid? CenovusProjectId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public Guid FacilityId { get; set; }
        [Required(ErrorMessage = "This field is required.")]
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
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        public Guid? CopyInsulationTableDefaultsEpProjectId { get; set; }
    }
}