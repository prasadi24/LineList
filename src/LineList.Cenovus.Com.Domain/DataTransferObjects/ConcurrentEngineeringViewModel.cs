using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class ConcurrentEngineeringViewModel
    {
        public Guid FacilityId { get; set; }
        public Guid EPProjectId { get; set; }

        public bool OnlyShowAsBuiltLDTs { get; set; }
        public DateTime? LDTFromDate { get; set; }
        public DateTime? LDTToDate { get; set; }

        public List<Facility> Facilities { get; set; }
        public List<EpProject> EPProjects { get; set; }

        public bool IsCenovusAdmin { get; set; }
    }
}