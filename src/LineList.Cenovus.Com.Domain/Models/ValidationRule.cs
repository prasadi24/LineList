namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    public class ValidationRule : Entity
    {
        public Guid ValidationId { get; set; }
        public virtual Validation Validation { get; set; }

        public string FieldName { get; set; }
        public int RuleType { get; set; }
        public int RuleOperator { get; set; }
        public int SortOrder { get; set; }
        public string Value { get; set; }
        public string Criteria { get; set; }
    }
}