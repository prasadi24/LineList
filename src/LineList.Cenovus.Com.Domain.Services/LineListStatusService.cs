using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LineListStatusService : ILineListStatusService
    {
        private readonly ILineListStatusRepository _lineListStatusRepository;

        public LineListStatusService(ILineListStatusRepository lineListStatusRepository)
        {
            _lineListStatusRepository = lineListStatusRepository;
        }

        public async Task<IEnumerable<LineListStatus>> GetAll()
        {
            return await _lineListStatusRepository.GetAll();
        }

        public async Task<LineListStatus> GetById(Guid id)
        {
            return await _lineListStatusRepository.GetById(id);
        }
        public async Task<Guid?> GetCancelledDraftId()
        {
            return await _lineListStatusRepository.GetCancelledDraftId();
        }
        public async Task<LineListStatus> Add(LineListStatus lineListStatus)
        {
            // Prevent adding a duplicate LineListStatus entry based on Name
            if (_lineListStatusRepository.Search(c => c.Name == lineListStatus.Name).Result.Any())
                return null;

            await _lineListStatusRepository.Add(lineListStatus);
            return lineListStatus;
        }

        public async Task<LineListStatus> Update(LineListStatus lineListStatus)
        {
            // Prevent updating to a duplicate LineListStatus entry based on Name
            if (_lineListStatusRepository.Search(c => c.Name == lineListStatus.Name && c.Id != lineListStatus.Id).Result.Any())
                return null;

            await _lineListStatusRepository.Update(lineListStatus);
            return lineListStatus;
        }

        public async Task<bool> Remove(LineListStatus lineListStatus)
        {
            await _lineListStatusRepository.Remove(lineListStatus);
            return true;
        }

        public async Task<IEnumerable<LineListStatus>> Search(string searchCriteria)
        {
            return await _lineListStatusRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _lineListStatusRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _lineListStatusRepository.HasDependencies(id);
        }
    }
}