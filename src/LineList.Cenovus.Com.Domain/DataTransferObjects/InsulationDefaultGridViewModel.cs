namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class InsulationDefaultGridViewModel
    {
        public Guid RowId { get; set; }
        public string Nps { get; set; } // Size NPS
        public List<InsulationDefaultColumnViewModel> Columns { get; set; } = new List<InsulationDefaultColumnViewModel>();
    }

    public class EpProjectInsulationDefaultGridViewModel
    {
        public Guid RowId { get; set; }
        public string Nps { get; set; } // Size NPS
        public List<EpProjectInsulationDefaultColumnViewModel> Columns { get; set; } = new List<EpProjectInsulationDefaultColumnViewModel>();
    }
}