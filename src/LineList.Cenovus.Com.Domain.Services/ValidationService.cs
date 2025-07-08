using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IValidationRepository _validationRepository;

        public ValidationService(IValidationRepository validationRepository)
        {
            _validationRepository = validationRepository;
        }

        public async Task<IEnumerable<Validation>> GetAll()
        {
            return await _validationRepository.GetAll();
        }

        public async Task<Validation> GetById(Guid id)
        {
            return await _validationRepository.GetById(id);
        }

        public async Task<Validation> Add(Validation validation)
        {
            if (_validationRepository.Search(c => c.FieldName == validation.FieldName).Result.Any())
                return null;

            await _validationRepository.Add(validation);
            return validation;
        }

        public async Task<Validation> Update(Validation validation)
        {
            if (_validationRepository.Search(c => c.FieldName == validation.FieldName && c.Id != validation.Id).Result.Any())
                return null;

            await _validationRepository.Update(validation);
            return validation;
        }

        public async Task<bool> Remove(Validation validation)
        {
            await _validationRepository.Remove(validation);
            return true;
        }

        public async Task<IEnumerable<Validation>> Search(string searchCriteria)
        {
            return await _validationRepository.Search(c => c.FieldName.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _validationRepository?.Dispose();
        }
    }
}