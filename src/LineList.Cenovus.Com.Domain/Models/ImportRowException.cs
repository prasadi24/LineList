using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ImportRowException")]
    public class ImportRowException : Entity
    {
        public string Message { get; set; }

        //Foreign Keys
        public virtual ImportRow ImportRow { get; set; }

        public Guid ImportRowId { get; set; }
        public int? SegmentNumber { get; set; }
    }
}