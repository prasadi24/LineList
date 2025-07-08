using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
	[Serializable]
	[Table("InsulationDefault")]
	public class InsulationDefault : LLTable
	{
		public Guid? InsulationMaterialId { get; set; }
		public Guid? InsulationTypeId { get; set; }
		public Guid? TracingTypeId { get; set; }

		public string Description { get; set; }
		public string SpecificationRevision { get; set; }
		public Nullable<DateTime> SpecificationRevisionDate { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public string Name
        {
            get => _name?.ToUpper();           
            set => _name = value?.ToUpper();   
        }
        private string _name;

        public string? Notes { get; set; }
        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }
		[Display(Name= "Link")]
		public string? LinkToDocument { get; set; }

		public virtual InsulationMaterial InsulationMaterial { get; set; }
		public virtual InsulationType InsulationType { get; set; }
		public virtual TracingType TracingType { get; set; }

		public virtual ICollection<InsulationDefaultColumn> Columns { get; set; }
		public virtual ICollection<InsulationDefaultRow> Rows { get; set; }

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