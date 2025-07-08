using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("LineListStatus")]
    public class LineListStatus : LLLookupTable
    {
        [ForeignKey("IsIssuedOf")]
        public Guid? IsIssuedOfId { get; set; }

        public virtual LineListStatus IsIssuedOf { get; set; }

        [ForeignKey("IsDraftOf")]
        public Guid? IsDraftOfId { get; set; }

        public virtual LineListStatus IsDraftOf { get; set; }

        public bool IsHardRevision { get; set; }

        [ForeignKey("CorrespondingLineStatus")]
        public Guid? CorrespondingLineStatusId { get; set; }

        public virtual LineStatus CorrespondingLineStatus { get; set; }

        [ForeignKey("DefaultUpRevStatus")]
        public Guid? DefaultUpRevStatusId { get; set; }

        public virtual LineListStatus DefaultUpRevStatus { get; set; }

        [NotMapped]
        public string IsHardRevisionText
        { get { return this.IsHardRevision ? "Yes" : "No"; } }
    }
}