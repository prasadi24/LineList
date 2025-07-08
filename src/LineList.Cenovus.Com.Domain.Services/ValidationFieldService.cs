using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ValidationFieldService : IValidationFieldService
    {
        private readonly IValidationFieldRepository _validationFieldRepository;

        public ValidationFieldService(IValidationFieldRepository validationFieldRepository)
        {
            _validationFieldRepository = validationFieldRepository;
        }

        public async Task<IEnumerable<ValidationField>> GetAll()
        {
            return await _validationFieldRepository.GetAll();
        }

        public async Task<ValidationField> GetById(Guid id)
        {
            return await _validationFieldRepository.GetById(id);
        }

        public async Task<ValidationField> Add(ValidationField validationField)
        {
            // Check if a validation field with the same name exists
            if (_validationFieldRepository.Search(c => c.FieldName == validationField.FieldName).Result.Any())
                return null;

            await _validationFieldRepository.Add(validationField);
            return validationField;
        }

        public async Task<ValidationField> Update(ValidationField validationField)
        {
            // Ensure that no other validation field with the same name exists
            if (_validationFieldRepository.Search(c => c.FieldName == validationField.FieldName && c.Id != validationField.Id).Result.Any())
                return null;

            await _validationFieldRepository.Update(validationField);
            return validationField;
        }

        public async Task<bool> Remove(ValidationField validationField)
        {
            await _validationFieldRepository.Remove(validationField);
            return true;
        }

        public async Task<IEnumerable<ValidationField>> Search(string searchCriteria)
        {
            return await _validationFieldRepository.Search(c => c.FieldName.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _validationFieldRepository?.Dispose();
        }
    }
}