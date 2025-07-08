using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("EpProjectInsulationDefaultRow")]
    public class EpProjectInsulationDefaultRow : LLTable
    {
        public Guid? SizeNpsId { get; set; }
        public Guid EpProjectInsulationDefaultId { get; set; }

        public virtual EpProjectInsulationDefault EpProjectInsulationDefault { get; set; }
        public virtual SizeNps SizeNps { get; set; }
    }
}