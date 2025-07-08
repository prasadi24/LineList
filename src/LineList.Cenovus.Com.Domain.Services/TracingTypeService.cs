using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class TracingTypeService : ITracingTypeService
    {
        private readonly ITracingTypeRepository _tracingTypeRepository;

        public TracingTypeService(ITracingTypeRepository tracingTypeRepository)
        {
            _tracingTypeRepository = tracingTypeRepository;
        }

        public async Task<IEnumerable<TracingType>> GetAll()
        {
            return await _tracingTypeRepository.GetAll();
        }

        public async Task<TracingType> GetById(Guid id)
        {
            return await _tracingTypeRepository.GetById(id);
        }

        public async Task<TracingType> Add(TracingType tracingType)
        {
            // Prevent adding duplicate TracingType based on name
            if (_tracingTypeRepository.Search(c => c.Name == tracingType.Name && c.SpecificationId == tracingType.SpecificationId).Result.Any())
                return null;  // Prevent duplicate TracingType

            await _tracingTypeRepository.Add(tracingType);
            return tracingType;
        }

        public async Task<TracingType> Update(TracingType tracingType)
        {
            // Prevent updating to a duplicate TracingType for another entity
            if (_tracingTypeRepository.Search(c => c.Name == tracingType.Name && c.SpecificationId == tracingType.SpecificationId && c.Id != tracingType.Id).Result.Any())
                return null;  // Prevent updating to a duplicate name

            await _tracingTypeRepository.Update(tracingType);
            return tracingType;
        }

        public async Task<bool> Remove(TracingType tracingType)
        {
            await _tracingTypeRepository.Remove(tracingType);
            return true;
        }

        public async Task<IEnumerable<TracingType>> Search(string searchCriteria)
        {
            return await _tracingTypeRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _tracingTypeRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _tracingTypeRepository.HasDependencies(id);
        }
    }
}