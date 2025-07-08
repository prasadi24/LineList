using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ImportFacility
{
    public class ImportFacilityEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid ImportId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid FacilityId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int BeforeCount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int AfterCount { get; set; }
    }
}