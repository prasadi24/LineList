using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportLocationService : IImportLocationService
    {
        private readonly IImportLocationRepository _importLocationRepository;

        public ImportLocationService(IImportLocationRepository importLocationRepository)
        {
            _importLocationRepository = importLocationRepository;
        }

        public async Task<IEnumerable<ImportLocation>> GetAll()
        {
            return await _importLocationRepository.GetAll();
        }

        public async Task<ImportLocation> GetById(Guid id)
        {
            return await _importLocationRepository.GetById(id);
        }

        public async Task<ImportLocation> Add(ImportLocation importLocation)
        {
            // Example: Check for duplicate import location
            if (_importLocationRepository.Search(c => c.Id == importLocation.Id).Result.Any())
                return null;

            await _importLocationRepository.Add(importLocation);
            return importLocation;
        }

        public async Task<ImportLocation> Update(ImportLocation importLocation)
        {
            // Example: Check for duplicate name while updating
            if (_importLocationRepository.Search(c => c.Id == importLocation.Id && c.Id != importLocation.Id).Result.Any())
                return null;

            await _importLocationRepository.Update(importLocation);
            return importLocation;
        }

        public async Task<bool> Remove(ImportLocation importLocation)
        {
            await _importLocationRepository.Remove(importLocation);
            return true;
        }

        //public async Task<IEnumerable<ImportLocation>> Search(string searchCriteria)
        //{
        //    return await _importLocationRepository.Search(c => c.Id.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _importLocationRepository?.Dispose();
        }
    }
}