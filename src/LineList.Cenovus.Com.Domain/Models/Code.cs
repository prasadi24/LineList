using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("Code")]
    public class Code : LLLookupTable
    {
        public bool IsCsa { get; set; }

        [NotMapped]
        public string IsCsaText
        { get { return this.IsCsa ? "Yes" : "No"; } }
    }
}