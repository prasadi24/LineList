using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportSheet
{
    public class ImportSheetAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid ImportId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(10, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string SheetType { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int NumberOfRows { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int NumberOfExceptions { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int NumberOfColumns { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int NumberOfAccepted { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int NumberOfImported { get; set; }

        public string? IgnoredFields { get; set; }
    }
}