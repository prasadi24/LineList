using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.API.DataTransferObjects.ValidationRule
{
    public class ValidationRuleEditDto
    {
        [Required(ErrorMessage = "This field is required.")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public Guid ValidationId { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        [StringLength(50, ErrorMessage = "This field length must be between {2} and {1} characters.", MinimumLength = 2)]
        public string FieldName { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int RuleType { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int RuleOperator { get; set; }

        [StringLength(50, ErrorMessage = "This field cannot exceed {1} characters.")]
        public string? Value { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public int SortOrder { get; set; }

        public string? Criteria { get; set; }
    }
}