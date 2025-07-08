namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class ConcurrentEngineeringReportViewModel
    {
        public Guid LineListRevisionId { get; set; }
        public string? Title { get; set; }
    }
    public class ActiveSetting
    {
        public Guid LineRevisionId { get; set; }
        public bool IsActive { get; set; }
    }
}
