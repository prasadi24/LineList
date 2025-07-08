using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Role
{
	public class RoleAddDto
	{
		[Required(ErrorMessage = "This field is required.")]
		[StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
		public string Description { get; set; }

		public string CreatedBy { get; set; }
		public string ModifiedBy { get; set; }
	}
}