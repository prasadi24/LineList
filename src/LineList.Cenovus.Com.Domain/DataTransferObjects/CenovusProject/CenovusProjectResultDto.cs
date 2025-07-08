using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.CenovusProject
{
    public class CenovusProjectResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
        public string FacilityName { get; set; }

        [Display(Name = "Project Type")]
        public string ProjectTypeName { get; set; }


        [Display(Name = "Sort")] 
        public int SortOrder { get; set; }

        [Display(Name = "Active")]

        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid FacilityId { get; set; }

        public Guid ProjectTypeId { get; set; }
    }
}