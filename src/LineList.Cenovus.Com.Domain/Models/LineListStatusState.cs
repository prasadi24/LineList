using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("LineListStatusState")]
    public class LineListStatusState : Entity
    {
        public Guid CurrentStatusId { get; set; }
        public Guid FutureStatusId { get; set; }

        public Guid? RequiredIssuedStatus1Id { get; set; }
        public Guid? RequiredIssuedStatus2Id { get; set; }
        public Guid? RequiredIssuedStatus3Id { get; set; }

        public Guid? ExcludeIssuedStatus1Id { get; set; }
        public Guid? ExcludeIssuedStatus2Id { get; set; }
        public Guid? ExcludeIssuedStatus3Id { get; set; }
        public Guid? ExcludeIssuedStatus4Id { get; set; }

        public bool IsForUpRev { get; set; }

        // Navigation Properties (Fixing CS0452)
        public virtual LineListStatus CurrentStatus { get; set; } = null!;

        public virtual LineListStatus FutureStatus { get; set; } = null!;
        public virtual LineListStatus? RequiredIssuedStatus1 { get; set; }
        public virtual LineListStatus? RequiredIssuedStatus2 { get; set; }
        public virtual LineListStatus? RequiredIssuedStatus3 { get; set; }
        public virtual LineListStatus? ExcludeIssuedStatus1 { get; set; }
        public virtual LineListStatus? ExcludeIssuedStatus2 { get; set; }
        public virtual LineListStatus? ExcludeIssuedStatus3 { get; set; }
        public virtual LineListStatus? ExcludeIssuedStatus4 { get; set; }
    }
}