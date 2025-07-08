using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class InsulationDefaultDetailsEditViewModel
    {

        public Guid Id { get; set; }
        public Guid InsulationDefaultId { get; set; }
        public string MinOperatingTemperature { get; set; }
        public string MaxOperatingTemperature { get; set; }
        public string TracingType { get; set; }
        public Guid? SizeNpsId { get; set; }
        public Guid? InsulationThicknessId { get; set; }
        public Guid? InsulationDefaultColumnId { get; set; }
        public Guid InsulationDefaultRowId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public List<SizeNps> SizeNps_s { get; set; }
        public List<InsulationThickness> insulationThicknesses { get; set; }
    }
}
