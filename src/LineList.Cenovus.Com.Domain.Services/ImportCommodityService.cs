using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportCommodityService : IImportCommodityService
    {
        private readonly IImportCommodityRepository _importCommodityRepository;

        public ImportCommodityService(IImportCommodityRepository importCommodityRepository)
        {
            _importCommodityRepository = importCommodityRepository;
        }

        public async Task<IEnumerable<ImportCommodity>> GetAll()
        {
            return await _importCommodityRepository.GetAll();
        }

        public async Task<ImportCommodity> GetById(Guid id)
        {
            return await _importCommodityRepository.GetById(id);
        }

        public async Task<ImportCommodity> Add(ImportCommodity importCommodity)
        {
            // Example check for duplicate commodity name
            if (_importCommodityRepository.Search(c => c.Id == importCommodity.Id).Result.Any())
                return null;

            await _importCommodityRepository.Add(importCommodity);
            return importCommodity;
        }

        public async Task<ImportCommodity> Update(ImportCommodity importCommodity)
        {
            // Example check for duplicate name while updating
            if (_importCommodityRepository.Search(c => c.Id == importCommodity.Id && c.Id != importCommodity.Id).Result.Any())
                return null;

            await _importCommodityRepository.Update(importCommodity);
            return importCommodity;
        }

        public async Task<bool> Remove(ImportCommodity importCommodity)
        {
            await _importCommodityRepository.Remove(importCommodity);
            return true;
        }

        //public async Task<IEnumerable<ImportCommodity>> Search(string searchCriteria)
        //{
        //    return await _importCommodityRepository.Search(c => c.Name.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _importCommodityRepository?.Dispose();
        }
    }
}