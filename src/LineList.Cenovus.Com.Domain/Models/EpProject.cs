using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("EpProject")]
    public class EpProject : LLLookupTable
    {
        public Guid? CenovusProjectId { get; set; }
        public Guid EpCompanyId { get; set; }

        public Guid? FacilityId { get; set; }

        public Guid? ProjectTypeId { get; set; }

        public Guid? CopyInsulationTableDefaultsEpProjectId { get; set; }

        public virtual CenovusProject CenovusProject { get; set; }
        public virtual EpCompany EpCompany { get; set; }

        public virtual ICollection<EpProjectUserRole> EpProjectUsers { get; set; }
        public virtual ICollection<EpProjectInsulationDefault> EpProjectInsulationDefaults { get; set; }
      
    }
}