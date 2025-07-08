namespace LineList.Cenovus.Com.API.DataTransferObjects.Validation
{
    public class ValidationAddDto
    {
        public int? ValidationType { get; set; }

        public int? RuleNumber { get; set; }

        public string? Message { get; set; }

        public string? FieldName { get; set; }
    }
}