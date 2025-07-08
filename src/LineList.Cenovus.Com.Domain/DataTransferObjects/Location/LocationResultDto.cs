using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Location
{
    public class LocationResultDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Facility")]
        public string FacilityName { get; set; }

        [Display(Name = "Location Type")]
        public string LocationTypeName { get; set; }

        public string? Description { get; set; }

        public string? Notes { get; set; }

        [Display(Name = "Sort")]
        public int SortOrder { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public Guid FacilityId { get; set; }

        public Guid LocationTypeId { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}