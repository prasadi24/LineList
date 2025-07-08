using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("InsulationDefaultColumn")]
    public class InsulationDefaultColumn : LLTable
    {
        public Guid InsulationDefaultId { get; set; }
        public int? MinOperatingTemperature { get; set; }
        public int? MaxOperatingTemperature { get; set; }

        public virtual InsulationDefault InsulationDefault { get; set; }
    }
}