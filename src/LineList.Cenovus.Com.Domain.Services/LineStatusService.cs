using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LineStatusService : ILineStatusService
    {
        private readonly ILineStatusRepository _lineStatusRepository;

        public LineStatusService(ILineStatusRepository lineStatusRepository)
        {
            _lineStatusRepository = lineStatusRepository;
        }

        public async Task<IEnumerable<LineStatus>> GetAll()
        {
            return await _lineStatusRepository.GetAll();
        }

        public async Task<LineStatus> GetById(Guid id)
        {
            return await _lineStatusRepository.GetById(id);
        }
        public async Task<Guid> GetStatusIdByName(string name)
        {
            return await _lineStatusRepository.GetStatusIdByName(name);
        }

        public async Task<Guid> GetDeletedStatusId()
        {
            return await _lineStatusRepository.GetDeletedStatusId();
        }

        public async Task<LineStatus> Add(LineStatus lineStatus)
        {
            // Check if the LineStatus already exists by its Name or other unique property
            if (_lineStatusRepository.Search(c => c.Name == lineStatus.Name).Result.Any())
                return null;

            await _lineStatusRepository.Add(lineStatus);
            return lineStatus;
        }

        public async Task<LineStatus> Update(LineStatus lineStatus)
        {
            // Check for duplicates or other custom logic, if necessary
            if (_lineStatusRepository.Search(c => c.Name == lineStatus.Name && c.Id != lineStatus.Id).Result.Any())
                return null;

            await _lineStatusRepository.Update(lineStatus);
            return lineStatus;
        }

        public async Task<bool> Remove(LineStatus lineStatus)
        {
            await _lineStatusRepository.Remove(lineStatus);
            return true;
        }

        public async Task<IEnumerable<LineStatus>> Search(string searchCriteria)
        {
            return await _lineStatusRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _lineStatusRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _lineStatusRepository.HasDependencies(id);
        }
    }
}