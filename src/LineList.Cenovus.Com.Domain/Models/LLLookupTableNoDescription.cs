using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    public class LLLookupTableNoDescription : LLTable
    {
        public string Name
        {
            get => _name?.ToUpper();
            set => _name = value?.ToUpper();
        }
        private string _name;

        public bool IsActive { get; set; }
        public string? Notes { get; set; }
        public int SortOrder { get; set; }

        [NotMapped]
        public string IsActiveText
        { get { return this.IsActive ? "Yes" : "No"; } }
    }
}