using System.ComponentModel.DataAnnotations;

namespace LineList.Cenovus.Com.Domain.Models
{
    public abstract class Entity
    {
        [Key]
        public Guid Id { get; set; }
    }
}