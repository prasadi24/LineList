using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectUserRole
{
    public class EpProjectUserRoleEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpProjectId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid EpProjectRoleId { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,TimeZoneInfo.FindSystemTimeZoneById("Mountain Standard Time"));

        public string EpCompanyName { get; set; }
        public string EpProjectName { get; set; }
    }
}