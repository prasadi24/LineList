using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportSheetColumnService : IImportSheetColumnService
    {
        private readonly IImportSheetColumnRepository _importSheetColumnRepository;

        public ImportSheetColumnService(IImportSheetColumnRepository importSheetColumnRepository)
        {
            _importSheetColumnRepository = importSheetColumnRepository;
        }

        public async Task<IEnumerable<ImportSheetColumn>> GetAll()
        {
            return await _importSheetColumnRepository.GetAll();
        }

        public async Task<ImportSheetColumn> GetById(Guid id)
        {
            return await _importSheetColumnRepository.GetById(id);
        }

        public async Task<ImportSheetColumn> Add(ImportSheetColumn importSheetColumn)
        {
            if (_importSheetColumnRepository.Search(c => c.ImportSheet == importSheetColumn.ImportSheet).Result.Any())
                return null;

            await _importSheetColumnRepository.Add(importSheetColumn);
            return importSheetColumn;
        }

        public async Task<ImportSheetColumn> Update(ImportSheetColumn importSheetColumn)
        {
            if (_importSheetColumnRepository.Search(c => c.ImportSheet == importSheetColumn.ImportSheet && c.Id != importSheetColumn.Id).Result.Any())
                return null;

            await _importSheetColumnRepository.Update(importSheetColumn);
            return importSheetColumn;
        }

        public async Task<bool> Remove(ImportSheetColumn importSheetColumn)
        {
            await _importSheetColumnRepository.Remove(importSheetColumn);
            return true;
        }

        //public async Task<IEnumerable<ImportSheetColumn>> Search(string searchCriteria)
        //{
        //    return await _importSheetColumnRepository.Search(c => c.ImportSheet.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _importSheetColumnRepository?.Dispose();
        }
    }
}