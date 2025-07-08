using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.EpProjectInsulationDefaultColumn
{
    public class EpProjectInsulationDefaultColumnResultDto
    {
        public Guid Id { get; set; }
        [Display(Name = "Operating Minimum")]
        public double? MinOperatingTemperature { get; set; }
        [Display(Name = "Operating Maximum")]
        public double? MaxOperatingTemperature { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid EpProjectInsulationDefaultId { get; set; }
    }
}