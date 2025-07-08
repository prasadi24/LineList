using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class PipeSpecificationService : IPipeSpecificationService
    {
        private readonly IPipeSpecificationRepository _pipeSpecificationRepository;

        public PipeSpecificationService(IPipeSpecificationRepository pipeSpecificationRepository)
        {
            _pipeSpecificationRepository = pipeSpecificationRepository;
        }

        public async Task<IEnumerable<PipeSpecification>> GetAll()
        {
            return await _pipeSpecificationRepository.GetAll();
        }

        public async Task<PipeSpecification> GetById(Guid id)
        {
            return await _pipeSpecificationRepository.GetById(id);
        }

        public async Task<PipeSpecification> Add(PipeSpecification pipeSpecification)
        {
            if (_pipeSpecificationRepository.Search(c => c.Name == pipeSpecification.Name && c.SpecificationId==pipeSpecification.SpecificationId).Result.Any())
                return null;

            await _pipeSpecificationRepository.Add(pipeSpecification);
            return pipeSpecification;
        }

        public async Task<PipeSpecification> Update(PipeSpecification pipeSpecification)
        {
            if (_pipeSpecificationRepository.Search(c => c.Name == pipeSpecification.Name && c.SpecificationId == pipeSpecification.SpecificationId && c.Id != pipeSpecification.Id).Result.Any())
                return null;

            await _pipeSpecificationRepository.Update(pipeSpecification);
            return pipeSpecification;
        }

        public async Task<bool> Remove(PipeSpecification pipeSpecification)
        {
            await _pipeSpecificationRepository.Remove(pipeSpecification);
            return true;
        }

        public async Task<IEnumerable<PipeSpecification>> Search(string searchCriteria)
        {
            return await _pipeSpecificationRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _pipeSpecificationRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _pipeSpecificationRepository.HasDependencies(id);
        }
    }
}