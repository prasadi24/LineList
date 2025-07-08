using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ImportSheetColumn")]
    public class ImportSheetColumn : Entity
    {
        public Guid ImportSheetId { get; set; }
        public virtual ImportSheet ImportSheet { get; set; }

        public string NameInExcel { get; set; }
        public string NameInDatabase { get; set; }
        public int SortOrder { get; set; }
    }
}