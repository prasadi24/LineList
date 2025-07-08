using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LocationTypeService : ILocationTypeService
    {
        private readonly ILocationTypeRepository _locationTypeRepository;

        public LocationTypeService(ILocationTypeRepository locationTypeRepository)
        {
            _locationTypeRepository = locationTypeRepository;
        }

        public async Task<IEnumerable<LocationType>> GetAll()
        {
            return await _locationTypeRepository.GetAll();
        }

        public async Task<LocationType> GetById(Guid id)
        {
            return await _locationTypeRepository.GetById(id);
        }

        public async Task<LocationType> Add(LocationType locationType)
        {
            // Prevent adding a duplicate LocationType entry based on Name
            if (_locationTypeRepository.Search(c => c.Name == locationType.Name).Result.Any())
                return null;

            await _locationTypeRepository.Add(locationType);
            return locationType;
        }

        public async Task<LocationType> Update(LocationType locationType)
        {
            // Prevent updating to a duplicate LocationType entry based on Name
            if (_locationTypeRepository.Search(c => c.Name == locationType.Name && c.Id != locationType.Id).Result.Any())
                return null;

            await _locationTypeRepository.Update(locationType);
            return locationType;
        }

        public async Task<bool> Remove(LocationType locationType)
        {
            await _locationTypeRepository.Remove(locationType);
            return true;
        }

        public async Task<IEnumerable<LocationType>> Search(string searchCriteria)
        {
            return await _locationTypeRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _locationTypeRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _locationTypeRepository.HasDependencies(id);
        }
    }
}