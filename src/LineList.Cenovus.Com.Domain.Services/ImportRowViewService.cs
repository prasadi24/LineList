using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ImportRowViewService : IImportRowViewService
    {
        private readonly IImportRowViewRepository _importRowViewRepository;

        public ImportRowViewService(IImportRowViewRepository importRowViewRepository)
        {
            _importRowViewRepository = importRowViewRepository;
        }

        public async Task<IEnumerable<ImportRowView>> GetAll()
        {
            return await _importRowViewRepository.GetAll();
        }

        public async Task<ImportRowView> GetById(Guid id)
        {
            return await _importRowViewRepository.GetById(id);
        }

        public async Task<ImportRowView> Add(ImportRowView importRowView)
        {
            if (_importRowViewRepository.Search(c => c.Id == importRowView.Id).Result.Any())
                return null;

            await _importRowViewRepository.Add(importRowView);
            return importRowView;
        }

        public async Task<ImportRowView> Update(ImportRowView importRowView)
        {
            if (_importRowViewRepository.Search(c => c.Id == importRowView.Id).Result.Any())
                return null;

            await _importRowViewRepository.Update(importRowView);
            return importRowView;
        }

        public async Task<bool> Remove(ImportRowView importRowView)
        {
            await _importRowViewRepository.Remove(importRowView);
            return true;
        }

        //public async Task<IEnumerable<ImportRowView>> Search(string searchCriteria)
        //{
        //    return await _importRowViewRepository.Search(c => c.Name.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _importRowViewRepository?.Dispose();
        }
    }
}