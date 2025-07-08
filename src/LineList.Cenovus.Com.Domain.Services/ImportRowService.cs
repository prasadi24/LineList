using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportRowService : IImportRowService
    {
        private readonly IImportRowRepository _importRowRepository;

        public ImportRowService(IImportRowRepository importRowRepository)
        {
            _importRowRepository = importRowRepository;
        }

        public async Task<IEnumerable<ImportRow>> GetAll()
        {
            return await _importRowRepository.GetAll();
        }

        public async Task<ImportRow> GetById(Guid id)
        {
            return await _importRowRepository.GetById(id);
        }

        public async Task<ImportRow> Add(ImportRow importRow)
        {
            // Example: Check for duplicate import row
            if (_importRowRepository.Search(c => c.ImportSheet == importRow.ImportSheet).Result.Any())
                return null;

            await _importRowRepository.Add(importRow);
            return importRow;
        }

        public async Task<ImportRow> Update(ImportRow importRow)
        {
            // Example: Check for duplicate name while updating
            if (_importRowRepository.Search(c => c.ImportSheet == importRow.ImportSheet && c.Id != importRow.Id).Result.Any())
                return null;

            await _importRowRepository.Update(importRow);
            return importRow;
        }

        public async Task<bool> Remove(ImportRow importRow)
        {
            await _importRowRepository.Remove(importRow);
            return true;
        }

        //public async Task<IEnumerable<ImportRow>> Search(string searchCriteria)
        //{
        //    return await _importRowRepository.Search(c => c.ImportSheet.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _importRowRepository?.Dispose();
        }
    }
}