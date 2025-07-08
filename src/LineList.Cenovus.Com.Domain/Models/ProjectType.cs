using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("ProjectType")]
    public class ProjectType : LLLookupTableNoDescription
    {
        public override string ToString()
        {
            return Name;
        }
    }
}