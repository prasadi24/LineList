using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportSheetColumn
{
    public class ImportSheetColumnAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid ImportSheetId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(255, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string NameInExcel { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(255, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string NameInDatabase { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int SortOrder { get; set; }
    }
}