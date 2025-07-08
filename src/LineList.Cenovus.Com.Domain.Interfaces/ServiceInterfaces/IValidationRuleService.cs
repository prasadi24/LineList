using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IValidationRuleService
    {
        Task<IEnumerable<ValidationRule>> GetAll();

        Task<ValidationRule> GetById(Guid id);

        Task<ValidationRule> Add(ValidationRule validationRule);

        Task<ValidationRule> Update(ValidationRule validationRule);

        Task<bool> Remove(ValidationRule validationRule);

        Task<IEnumerable<ValidationRule>> Search(string searchCriteria);
    }
}