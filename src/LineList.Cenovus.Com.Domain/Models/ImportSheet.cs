using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ImportSheet")]
    public class ImportSheet : Entity
    {
        public string Name { get; set; }
        public string SheetType { get; set; }
        public int NumberOfRows { get; set; }
        public int NumberOfExceptions { get; set; }
        public int NumberOfColumns { get; set; }
        public int NumberOfAccepted { get; set; }
        public int NumberOfImported { get; set; }

        public string IgnoredFields { get; set; }

        //foreign keys
        public virtual Import Import { get; set; }

        public Guid ImportId { get; set; }
        public virtual ICollection<ImportRow> ImportRows { get; set; }
        public virtual ICollection<ImportSheetColumn> ImportSheetColumns { get; set; }
    }
}