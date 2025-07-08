using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("EpProjectInsulationDefault")]
    public class EpProjectInsulationDefault : LLTable
    {
        public Guid EpProjectId { get; set; }
        public Guid? InsulationMaterialId { get; set; }
        public Guid? InsulationTypeId { get; set; }
        public Guid? TracingTypeId { get; set; }

        public string Description { get; set; }
        public string SpecificationRevision { get; set; }
        public Nullable<DateTime> SpecificationRevisionDate { get; set; }

        public bool IsActive { get; set; }

        [NotMapped]
        public string IsActiveText
        { get { return this.IsActive ? "Yes" : "No"; } }

        public string Name
        {
            get => _name?.ToUpper();
            set => _name = value?.ToUpper();
        }
        private string _name;
        public string? Notes { get; set; }
        public int SortOrder { get; set; }

        public string? LinkToDocument { get; set; }

        public virtual InsulationMaterial InsulationMaterial { get; set; }
        public virtual InsulationType InsulationType { get; set; }
        public virtual TracingType TracingType { get; set; }
        public virtual EpProject EpProject { get; set; }

        public virtual ICollection<EpProjectInsulationDefaultColumn> Columns { get; set; }
        public virtual ICollection<EpProjectInsulationDefaultRow> Rows { get; set; }

        [NotMapped]
        public string Name_dash_Description
        {
            get
            {
                if (Name == Description)
                {
                    return Name; // makes things look pretty, otherwise it looks like a duplicate
                }
                else
                {
                    return Name + " - " + Description;
                }
            }
        }
    }
}