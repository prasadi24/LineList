using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("Import")]
    public class Import : Entity
    {
        public string OriginalFileName { get; set; }
        public string FacilityName { get; set; }
        public string Status { get; set; }
        public string Path { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
        public int? ValidationCount { get; set; }

        public virtual ICollection<ImportSheet> ImportSheets { get; set; }
        public virtual ICollection<ImportFacility> ImportFacilities { get; set; }
        public virtual ICollection<ImportCommodity> ImportCommodities { get; set; }
        public virtual ICollection<ImportLocation> ImportLocations { get; set; }
    }
}