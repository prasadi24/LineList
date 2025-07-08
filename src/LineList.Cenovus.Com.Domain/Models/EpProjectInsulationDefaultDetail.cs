using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("EpProjectInsulationDefaultDetail")]
    public class EpProjectInsulationDefaultDetail : LLTable
    {
        public Guid EpProjectInsulationDefaultRowId { get; set; }
        public Guid EpProjectInsulationDefaultColumnId { get; set; }
        public Guid? InsulationThicknessId { get; set; }
        public Guid? TracingDesignNumberOfTracersId { get; set; }

        public virtual EpProjectInsulationDefaultRow EpProjectInsulationDefaultRow { get; set; }
        public virtual EpProjectInsulationDefaultColumn EpProjectInsulationDefaultColumn { get; set; }
        public virtual InsulationThickness InsulationThickness { get; set; }
        public virtual TracingDesignNumberOfTracers TracingDesignNumberOfTracers { get; set; }
    }
}