namespace LineList.Cenovus.Com.API.DataTransferObjects.ValidationRule
{
    public class ValidationRuleResultDto
    {
        public Guid Id { get; set; }

        public Guid ValidationId { get; set; }

        public string FieldName { get; set; }

        public int RuleType { get; set; }

        public int RuleOperator { get; set; }

        public string? Value { get; set; }

        public int SortOrder { get; set; }

        public string? Criteria { get; set; }
    }
}