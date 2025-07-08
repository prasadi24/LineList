using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class TurnoverViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public Guid FromEpCompanyId { get; set; }
        public string FromEpCompanyName { get; set; }
        public Guid ToEpCompanyId { get; set; }
        public IEnumerable<EpCompany> EpCompanies { get; set; }
        public bool IsCenovusAdmin { get; set; }
    }
}