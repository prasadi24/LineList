namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    public class RoleUser : LLTable
    {
        public string UserName { get; set; }
        public Guid RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}