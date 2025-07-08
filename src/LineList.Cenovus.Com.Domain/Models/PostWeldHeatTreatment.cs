using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("PostWeldHeatTreatment")]
    public class PostWeldHeatTreatment : LLLookupTableNoDescription
    {
        public override string ToString()
        {
            return Name;
        }
    }
}