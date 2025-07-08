using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportSheetService : IImportSheetService
    {
        private readonly IImportSheetRepository _importSheetRepository;

        public ImportSheetService(IImportSheetRepository importSheetRepository)
        {
            _importSheetRepository = importSheetRepository;
        }

        public async Task<IEnumerable<ImportSheet>> GetAll()
        {
            return await _importSheetRepository.GetAll();
        }

        public async Task<ImportSheet> GetById(Guid id)
        {
            return await _importSheetRepository.GetById(id);
        }

        public async Task<ImportSheet> Add(ImportSheet importSheet)
        {
            if (_importSheetRepository.Search(c => c.Name == importSheet.Name).Result.Any())
                return null;

            await _importSheetRepository.Add(importSheet);
            return importSheet;
        }

        public async Task<ImportSheet> Update(ImportSheet importSheet)
        {
            if (_importSheetRepository.Search(c => c.Name == importSheet.Name && c.Id != importSheet.Id).Result.Any())
                return null;

            await _importSheetRepository.Update(importSheet);
            return importSheet;
        }

        public async Task<bool> Remove(ImportSheet importSheet)
        {
            await _importSheetRepository.Remove(importSheet);
            return true;
        }

        public async Task<IEnumerable<ImportSheet>> Search(string searchCriteria)
        {
            return await _importSheetRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _importSheetRepository?.Dispose();
        }
    }
}