using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.Validation
{
    public class ValidationEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        public int? ValidationType { get; set; }

        public int? RuleNumber { get; set; }

        public string? Message { get; set; }

        public string? FieldName { get; set; }
    }
}