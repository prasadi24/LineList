namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    public class Validation : Entity
    {
        public int? ValidationType { get; set; }
        public int? RuleNumber { get; set; }
        public string Message { get; set; }
        public string FieldName { get; set; }

        public virtual List<ValidationRule> Rules { get; set; }
    }
}