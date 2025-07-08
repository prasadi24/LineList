using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportRowExceptionService : IImportRowExceptionService
    {
        private readonly IImportRowExceptionRepository _importRowExceptionRepository;

        public ImportRowExceptionService(IImportRowExceptionRepository importRowExceptionRepository)
        {
            _importRowExceptionRepository = importRowExceptionRepository;
        }

        public async Task<IEnumerable<ImportRowException>> GetAll()
        {
            return await _importRowExceptionRepository.GetAll();
        }

        public async Task<ImportRowException> GetById(Guid id)
        {
            return await _importRowExceptionRepository.GetById(id);
        }

        public async Task<ImportRowException> Add(ImportRowException importRowException)
        {
            if (_importRowExceptionRepository.Search(c => c.Id == importRowException.Id).Result.Any())
                return null;

            await _importRowExceptionRepository.Add(importRowException);
            return importRowException;
        }

        public async Task<ImportRowException> Update(ImportRowException importRowException)
        {
            if (_importRowExceptionRepository.Search(c => c.Id == importRowException.Id).Result.Any())
                return null;

            await _importRowExceptionRepository.Update(importRowException);
            return importRowException;
        }

        public async Task<bool> Remove(ImportRowException importRowException)
        {
            await _importRowExceptionRepository.Remove(importRowException);
            return true;
        }

        //public async Task<IEnumerable<ImportRowException>> Search(string searchCriteria)
        //{
        //    return await _importRowExceptionRepository.Search(c => c.Description.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _importRowExceptionRepository?.Dispose();
        }
    }
}