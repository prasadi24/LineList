using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LineList.Cenovus.Com.Domain.Models
{
    [Serializable]
    [Table("EpProjectUserRole")]
    public class EpProjectUserRole : LLTable
    {
        public Guid EpProjectId { get; set; }
        public Guid EpProjectRoleId { get; set; }
        [Display(Name="User Name")]
        public string UserName { get; set; }

        public virtual EpProject EpProject { get; set; }
        public virtual EpProjectRole EpProjectRole { get; set; }
    }
}