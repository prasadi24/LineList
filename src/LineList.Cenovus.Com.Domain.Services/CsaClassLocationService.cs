using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class CsaClassLocationService : ICsaClassLocationService
    {
        private readonly ICsaClassLocationRepository _csaClassLocationRepository;

        public CsaClassLocationService(ICsaClassLocationRepository csaClassLocationRepository)
        {
            _csaClassLocationRepository = csaClassLocationRepository;
        }

        public async Task<IEnumerable<CsaClassLocation>> GetAll()
        {
            return await _csaClassLocationRepository.GetAll();
        }

        public async Task<CsaClassLocation> GetById(Guid id)
        {
            return await _csaClassLocationRepository.GetById(id);
        }

        public async Task<CsaClassLocation> Add(CsaClassLocation csaClassLocation)
        {
            if (_csaClassLocationRepository.Search(c => c.Name == csaClassLocation.Name).Result.Any())
                return null;

            await _csaClassLocationRepository.Add(csaClassLocation);
            return csaClassLocation;
        }

        public async Task<CsaClassLocation> Update(CsaClassLocation csaClassLocation)
        {
            if (_csaClassLocationRepository.Search(c => c.Name == csaClassLocation.Name && c.Id != csaClassLocation.Id).Result.Any())
                return null;

            await _csaClassLocationRepository.Update(csaClassLocation);
            return csaClassLocation;
        }

        public async Task<bool> Remove(CsaClassLocation csaClassLocation)
        {
            await _csaClassLocationRepository.Remove(csaClassLocation);
            return true;
        }

        public async Task<IEnumerable<CsaClassLocation>> Search(string searchCriteria)
        {
            return await _csaClassLocationRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _csaClassLocationRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _csaClassLocationRepository.HasDependencies(id);
        }
    }
}