using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ScheduleDefault
{
    public class ScheduleDefaultResultDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Rev. #")]
        public int RevisionNumber { get; set; }

        [Display(Name = "Rev. Date")]
        public DateTime RevisionDate { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
        [Display(Name = "Sort Order")]
        public int? SortOrder { get; set; }

        public Guid PipeSpecificationId { get; set; }

        public Guid ScheduleId { get; set; }

        public Guid SizeNpsId { get; set; }
        public Guid SpecificationId { get; set; }

        [Display(Name = "Schedule Default")]
        public string ScheduleName { get; set; }

        [Display(Name = "Pipe Spec")]
        public string PipeSpecificationName { get; set; }

        [Display(Name = "Size NPS")]
        public string SizeNpsName { get; set; }
        
        [Display(Name = "Spec")]
        public string SpecificationName { get; set; }
    }
}