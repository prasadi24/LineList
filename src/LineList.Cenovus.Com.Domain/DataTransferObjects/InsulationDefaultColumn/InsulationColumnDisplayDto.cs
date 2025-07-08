using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects.InsulationDefaultColumn
{
    public class InsulationColumnDisplayDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Row")]
        public int RowNum { get; set; }

        [Display(Name = "Operating Minimum")]
        public double? MinOperatingTemperature { get; set; }

        [Display(Name = "Operating Maximum")]
        public double? MaxOperatingTemperature { get; set; }

        public Guid InsulationDefaultId { get; set; }
    }
}