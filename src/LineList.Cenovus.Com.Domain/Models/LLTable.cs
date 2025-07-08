namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    public class LLTable : Entity
    {
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}