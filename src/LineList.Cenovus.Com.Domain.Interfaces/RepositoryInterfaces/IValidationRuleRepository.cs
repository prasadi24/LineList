using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IValidationRuleRepository : IRepository<ValidationRule>
    {
        new Task<List<ValidationRule>> GetAll();

        new Task<ValidationRule> GetById(Guid id);
    }
}