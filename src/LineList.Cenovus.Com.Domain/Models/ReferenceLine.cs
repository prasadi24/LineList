namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    public class ReferenceLine : LLTable
    {
        public Guid LineRevisionId { get; set; }
        public Guid LineListRevisionId { get; set; }

        public virtual LineRevision LineRevision { get; set; }
        public virtual LineListRevision LineListRevision { get; set; }
    }
}