using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("EpCompanyAlpha")]
    public class EpCompanyAlpha : LLTable
    {
        public string Alpha { get; set; }

        public Guid FacilityId { get; set; }
        public Guid EpCompanyId { get; set; }

        public virtual EpCompany EpCompany { get; set; }
        public virtual Facility Facility { get; set; }
    }
}