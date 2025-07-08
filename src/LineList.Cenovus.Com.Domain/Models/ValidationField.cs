namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    public class ValidationField : Entity
    {
        public string FieldName { get; set; }
        public int FieldType { get; set; }
    }
}