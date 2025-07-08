using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("LineStatus")]
    public class LineStatus : LLLookupTableNoDescription
    {
        public bool IsDefaultForReservation { get; set; }

        [NotMapped]
        public string IsDefaultForReservationText
        { get { return this.IsDefaultForReservation ? "Yes" : "No"; } }
    }
}