using LineList.Cenovus.Com.API.DataTransferObjects.EpProject;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects
{
    public class EPProjectViewModel
    {
        public EpProjectAddDto EpProjectAdd { get; set; }
        public EpProjectEditDto EpProject { get; set; }
        public List<Facility> Facilities { get; set; }

        public List<CenovusProject> CenovusProjects { get; set; }

        public List<ProjectType> ProjectTypes { get; set; }

        public List<EpCompany> EpCompanies { get; set; }

        public List<InsulationDefault> InsulationDefaults { get; set; }
        public List<EpProjectInsulationDefault> EpProjectInsulationDefaults { get; set; }
        public Dictionary<Guid, string> InsulationDefaultsValues { get; set; }

        public List<EpProjectUserRole> EpProjectUserRoles { get; set; }

        public bool CanDelete { get; set; }
        public bool IsEpAdmin { get; set; }
        public bool CanAddRole { get; set; }
        public bool CanSave { get; set; }
        public bool CanChangeEp { get; set; }
        public bool CanBeTurnedOver { get; set; }
        public bool CanTurnover { get; set; }
        public bool CanChangeCenovusProject { get; set; }
        public bool CanChangeFacility { get; set; }
        public bool CanChangeDescription { get; set; }

    }
}