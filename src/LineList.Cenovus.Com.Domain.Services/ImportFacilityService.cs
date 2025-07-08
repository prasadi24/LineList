using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportFacilityService : IImportFacilityService
    {
        private readonly IImportFacilityRepository _importFacilityRepository;

        public ImportFacilityService(IImportFacilityRepository importFacilityRepository)
        {
            _importFacilityRepository = importFacilityRepository;
        }

        public async Task<IEnumerable<ImportFacility>> GetAll()
        {
            return await _importFacilityRepository.GetAll();
        }

        public async Task<ImportFacility> GetById(Guid id)
        {
            return await _importFacilityRepository.GetById(id);
        }

        public async Task<ImportFacility> Add(ImportFacility importFacility)
        {
            // Example: Check for duplicate import facility
            if (_importFacilityRepository.Search(c => c.Id == importFacility.Id).Result.Any())
                return null;

            await _importFacilityRepository.Add(importFacility);
            return importFacility;
        }

        public async Task<ImportFacility> Update(ImportFacility importFacility)
        {
            // Example: Check for duplicate name while updating
            if (_importFacilityRepository.Search(c => c.Id == importFacility.Id && c.Id != importFacility.Id).Result.Any())
                return null;

            await _importFacilityRepository.Update(importFacility);
            return importFacility;
        }

        public async Task<bool> Remove(ImportFacility importFacility)
        {
            await _importFacilityRepository.Remove(importFacility);
            return true;
        }

        //public async Task<IEnumerable<ImportFacility>> Search(string searchCriteria)
        //{
        //    return await _importFacilityRepository.Search(c => c.Id.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _importFacilityRepository?.Dispose();
        }
    }
}