using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("UserPreference")]
    public class UserPreference : Entity
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}