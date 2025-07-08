using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("EpProjectInsulationDefaultColumn")]
    public class EpProjectInsulationDefaultColumn : LLTable
    {
        public Guid EpProjectInsulationDefaultId { get; set; }
        public int? MinOperatingTemperature { get; set; }
        public int? MaxOperatingTemperature { get; set; }

        public virtual EpProjectInsulationDefault EpProjectInsulationDefault { get; set; }
    }
}