using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpCompany
{
    public class EpCompanyResultDto
    {
        public Guid Id { get; set; }

        [Display(Name = "EP Company Name")]
        public string Name { get; set; }

        [Display(Name = "EP Description")]
        public string? Description { get; set; }

        [Display(Name = "Active Directory Group")]
        public string ActiveDirectoryGroup { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Sort")]
        public int SortOrder { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        [Display(Name = "EP Company Logo")]
        public string? Logo { get; set; }
    }
}