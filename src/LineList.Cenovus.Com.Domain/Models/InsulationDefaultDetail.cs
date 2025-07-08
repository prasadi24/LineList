using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("InsulationDefaultDetail")]
    public class InsulationDefaultDetail : LLTable
    {
        public Guid InsulationDefaultRowId { get; set; }
        public Guid InsulationDefaultColumnId { get; set; }
        public Guid? InsulationThicknessId { get; set; }
        public Guid? TracingDesignNumberOfTracersId { get; set; }

        public virtual InsulationDefaultRow InsulationDefaultRow { get; set; }
        public virtual InsulationDefaultColumn InsulationDefaultColumn { get; set; }
        public virtual InsulationThickness InsulationThickness { get; set; }
        public virtual TracingDesignNumberOfTracers TracingDesignNumberOfTracers { get; set; }
    }
}