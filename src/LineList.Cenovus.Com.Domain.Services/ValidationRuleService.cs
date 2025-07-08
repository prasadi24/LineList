using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ValidationRuleService : IValidationRuleService
    {
        private readonly IValidationRuleRepository _validationRuleRepository;

        public ValidationRuleService(IValidationRuleRepository validationRuleRepository)
        {
            _validationRuleRepository = validationRuleRepository;
        }

        public async Task<IEnumerable<ValidationRule>> GetAll()
        {
            return await _validationRuleRepository.GetAll();
        }

        public async Task<ValidationRule> GetById(Guid id)
        {
            return await _validationRuleRepository.GetById(id);
        }

        public async Task<ValidationRule> Add(ValidationRule validationRule)
        {
            // Check if the validation rule already exists
            if (_validationRuleRepository.Search(c => c.FieldName == validationRule.FieldName).Result.Any())
                return null;

            await _validationRuleRepository.Add(validationRule);
            return validationRule;
        }

        public async Task<ValidationRule> Update(ValidationRule validationRule)
        {
            // Check if the validation rule with the same name already exists
            if (_validationRuleRepository.Search(c => c.FieldName == validationRule.FieldName && c.Id != validationRule.Id).Result.Any())
                return null;

            await _validationRuleRepository.Update(validationRule);
            return validationRule;
        }

        public async Task<bool> Remove(ValidationRule validationRule)
        {
            await _validationRuleRepository.Remove(validationRule);
            return true;
        }

        public async Task<IEnumerable<ValidationRule>> Search(string searchCriteria)
        {
            return await _validationRuleRepository.Search(c => c.FieldName.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _validationRuleRepository?.Dispose();
        }
    }
}