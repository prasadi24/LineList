using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("LineRevisionSegment")]
    public class LineRevisionSegment : LLTable
    {
        [ForeignKey("LineRevision")]
        [Required]
        public Guid LineRevisionId { get; set; }

        public virtual LineRevision LineRevision { get; set; }

        public Guid? InsulationMaterialId { get; set; }
        public virtual InsulationMaterial InsulationMaterial { get; set; }

        public Guid? InsulationThicknessId { get; set; }
        public virtual InsulationThickness InsulationThickness { get; set; }

        public Guid? InsulationTypeId { get; set; }
        public virtual InsulationType InsulationType { get; set; }

        public Guid? PaintSystemId { get; set; }
        public virtual PaintSystem PaintSystem { get; set; }

        public string SegmentNumber { get; set; }

        public Guid? SegmentTypeId { get; set; }
        public virtual SegmentType SegmentType { get; set; }

        public string? TracingDesignHoldTemperature { get; set; }

        public Guid? TracingDesignNumberOfTracersId { get; set; }
        public virtual TracingDesignNumberOfTracers TracingDesignNumberOfTracers { get; set; }

        public Guid? TracingTypeId { get; set; }
        public virtual TracingType TracingType { get; set; }
    }
}