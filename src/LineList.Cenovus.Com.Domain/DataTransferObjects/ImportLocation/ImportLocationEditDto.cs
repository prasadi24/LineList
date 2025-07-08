using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportLocation
{
    public class ImportLocationEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid ImportId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid LocationId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int BeforeCount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int AfterCount { get; set; }
    }
}