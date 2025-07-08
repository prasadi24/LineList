using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;

        public LocationService(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<Location>> GetAll()
        {
            return await _locationRepository.GetAll();
        }

        public async Task<Location> GetById(Guid id)
        {
            return await _locationRepository.GetById(id);
        }

        public async Task<Location> Add(Location location)
        {
            // Prevent adding a duplicate Location entry based on Name
            if (_locationRepository.Search(c => c.Name == location.Name && c.FacilityId==location.FacilityId).Result.Any())
                return null;

            await _locationRepository.Add(location);
            return location;
        }

        public async Task<Location> Update(Location location)
        {
            // Prevent updating to a duplicate Location entry based on Name
            if (_locationRepository.Search(c => c.Name == location.Name && c.Id != location.Id && c.FacilityId==location.FacilityId).Result.Any())
                return null;

            await _locationRepository.Update(location);
            return location;
        }

        public async Task<bool> Remove(Location location)
        {
            await _locationRepository.Remove(location);
            return true;
        }

        public async Task<IEnumerable<Location>> Search(string searchCriteria)
        {
            return await _locationRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _locationRepository?.Dispose();
        }
        public bool HasDependencies(Guid id)
        {
            return _locationRepository.HasDependencies(id);
        }
    }
}