using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("CenovusProject")]
    public class CenovusProject : LLTable
    {
        //note: for some reason this table doesn't
        //have a "Notes" column in the database...
        //so we can't inherit from LLLookupTable, only LLTable

        public string Name
        {
            get => _name?.ToUpper();
            set => _name = value?.ToUpper();
        }
        private string _name;
        public string Description { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public string IsActiveText
        { get { return this.IsActive ? "Yes" : "No"; } }

        public Guid FacilityId { get; set; }
        public Guid ProjectTypeId { get; set; }

        public virtual Facility Facility { get; set; }
        public virtual ProjectType ProjectType { get; set; }

        [NotMapped]
        public string Name_dash_Description
        {
            get
            {
                return LLLookupTable.FormatNameDescription(Name, Description);
            }
        }
    }
}