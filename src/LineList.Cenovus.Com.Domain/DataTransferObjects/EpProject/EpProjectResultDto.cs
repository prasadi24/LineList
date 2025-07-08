using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProject
{
	public class EpProjectResultDto
	{
		public Guid Id { get; set; }
		[Display(Name = "Project Number")]
		public string Name { get; set; }

		[Display(Name = "Project Description")]
		public string? Description { get; set; }

		public Guid? CenovusProjectId { get; set; }
		public Guid? FacilityId { get; set; }
		public Guid ProjectTypeId { get; set; }

		public Guid EpCompanyId { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

		public int SortOrder { get; set; }

		public string? Notes { get; set; }

		public string CreatedBy { get; set; }

		public DateTime CreatedOn { get; set; }

		public string? ModifiedBy { get; set; }

		public DateTime? ModifiedOn { get; set; }

		public Guid? CopyInsulationTableDefaultsEpProjectId { get; set; }

		[Display(Name = "Facility")]
		public string FacilityName { get; set; }

		[Display(Name = "Project Type")]
		public string ProjectTypeName { get; set; }

		[Display(Name = "Project Group / Expansion Phase")]
		public string CenovusProjectName { get; set; }

		[Display(Name = "EP")]
		public string EpCompanyName { get; set; }
	}
}