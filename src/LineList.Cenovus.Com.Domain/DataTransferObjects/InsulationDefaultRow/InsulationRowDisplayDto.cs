using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.Domain.DataTransferObjects.InsulationDefaultRow
{
    public class InsulationRowDisplayDto
    {
        public Guid Id { get; set; }

        [Display(Name = "Row")]
        public int RowNum { get; set; }

        [Display(Name = "Size NPS")]
        public string SizeNps { get; set; }

        public int SortOrder { get; set; }

        public Guid InsulationDefaultId { get; set; }
    }
}