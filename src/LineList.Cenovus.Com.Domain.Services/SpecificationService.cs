using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class SpecificationService : ISpecificationService
    {
        private readonly ISpecificationRepository _specificationRepository;

        public SpecificationService(ISpecificationRepository specificationRepository)
        {
            _specificationRepository = specificationRepository;
        }

        public async Task<IEnumerable<Specification>> GetAll()
        {
            return await _specificationRepository.GetAll();
        }

        public async Task<Specification> GetById(Guid id)
        {
            return await _specificationRepository.GetById(id);
        }

        public async Task<Specification> Add(Specification specification)
        {
            // Prevent adding a duplicate Specification entry based on Name
            if (_specificationRepository.Search(c => c.Name == specification.Name).Result.Any())
                return null;

            await _specificationRepository.Add(specification);
            return specification;
        }

        public async Task<Specification> Update(Specification specification)
        {
            // Prevent updating to a duplicate Specification entry based on Name
            if (_specificationRepository.Search(c => c.Name == specification.Name && c.Id != specification.Id).Result.Any())
                return null;

            await _specificationRepository.Update(specification);
            return specification;
        }

        public async Task<bool> Remove(Specification specification)
        {
            await _specificationRepository.Remove(specification);
            return true;
        }

        public async Task<IEnumerable<Specification>> Search(string searchCriteria)
        {
            return await _specificationRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _specificationRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _specificationRepository.HasDependencies(id);
        }
    }
}