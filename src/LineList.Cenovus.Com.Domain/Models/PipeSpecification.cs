using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("PipeSpecification")]
    public class PipeSpecification : LLLookupTable
    {
        public Guid SpecificationId { get; set; }
        public Guid? CorrosionAllowanceId { get; set; }
        public Guid? NdeCategoryId { get; set; }
        public Guid? XrayId { get; set; }
        public Guid? PostWeldHeatTreatmentId { get; set; }

        [Display(Name = "Rev")]
        public int? RevisionNumber { get; set; }

        public DateTime? RevisionDate { get; set; }

        public virtual Specification Specification { get; set; }
        public virtual CorrosionAllowance CorrosionAllowance { get; set; }
        public virtual NdeCategory NdeCategory { get; set; }
        public virtual Xray Xray { get; set; }
        public virtual PostWeldHeatTreatment PostWeldHeatTreatment { get; set; }
    }
}