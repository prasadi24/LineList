using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("LineList")]
    public class LineListModel : LLTable
    {
        public string DocumentNumber { get; set; }

        public virtual ICollection<LineListRevision> LineListRevisions { get; set; }
    }
}