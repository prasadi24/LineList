using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectUserRole
{
	public class EpProjectUserRoleAddDto
	{
		[Required(ErrorMessage = "This field is required.")]
		[StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
		public string UserName { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		public Guid EpProjectId { get; set; }

		[Required(ErrorMessage = "This field is required.")]
		public Guid EpProjectRoleId { get; set; }

		public string EpCompanyName { get; set; }
		public string EpProjectName { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string CreatedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime CreatedOn { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string ModifiedBy { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public DateTime ModifiedOn { get; set; }
    }
}