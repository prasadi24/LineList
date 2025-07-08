using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ValidationField
{
    public class ValidationFieldAddDto
    {
        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field length must be between {2} and {1} characters.", MinimumLength = 2)]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int FieldType { get; set; }
    }
}