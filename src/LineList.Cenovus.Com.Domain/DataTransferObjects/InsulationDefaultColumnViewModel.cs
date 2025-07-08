namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class InsulationDefaultColumnViewModel
    {
        public Guid ColumnId { get; set; }
        public string TemperatureRange { get; set; } // e.g., "0 to 100°C"
        public string InsulationThickness { get; set; }
        public string NumberOfTracers { get; set; }
        public string DetailsUrl { get; set; }
    }

    public class EpProjectInsulationDefaultColumnViewModel
    {
        public Guid ColumnId { get; set; }
        public string TemperatureRange { get; set; } // e.g., "0 to 100°C"
        public string InsulationThickness { get; set; }
        public string NumberOfTracers { get; set; }
        public string DetailsUrl { get; set; }
    }
}