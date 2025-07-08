using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LineRevisionOperatingModeService : ILineRevisionOperatingModeService
    {
        private readonly ILineRevisionOperatingModeRepository _lineRevisionOperatingModeRepository;

        public LineRevisionOperatingModeService(ILineRevisionOperatingModeRepository lineRevisionOperatingModeRepository)
        {
            _lineRevisionOperatingModeRepository = lineRevisionOperatingModeRepository;
        }

        public async Task<IEnumerable<LineRevisionOperatingMode>> GetAll()
        {
            return await _lineRevisionOperatingModeRepository.GetAll();
        }

        public async Task<LineRevisionOperatingMode> GetById(Guid id)
        {
            return await _lineRevisionOperatingModeRepository.GetById(id);
        }
        public async Task<List<LineRevisionOperatingMode>> GetOperatingModesByLineRevisionId(Guid lineRevisionId)
        {
            return await _lineRevisionOperatingModeRepository.GetOperatingModesByLineRevisionId(lineRevisionId);
        }
        public async Task<LineRevisionOperatingMode> GetPrimaryOperatingMode(Guid lineRevisionId)
        {
            return await _lineRevisionOperatingModeRepository.GetPrimaryOperatingMode(lineRevisionId);
        }

        public async Task<LineRevisionOperatingMode> Add(LineRevisionOperatingMode lineRevisionOperatingMode)
        {
            // Check if a LineRevisionOperatingMode with the same name already exists
            if (_lineRevisionOperatingModeRepository.Search(c => c.Id == lineRevisionOperatingMode.Id).Result.Any())
                return null;

            await _lineRevisionOperatingModeRepository.Add(lineRevisionOperatingMode);
            return lineRevisionOperatingMode;
        }
        public async Task<LineRevisionOperatingMode> AddWithoutSave(LineRevisionOperatingMode lineRevisionOperatingMode)
        {
            // Check if a LineRevisionOperatingMode with the same name already exists
            //if (_lineRevisionOperatingModeRepository.Search(c => c.Id == lineRevisionOperatingMode.Id).Result.Any())
            //    return null;

            await _lineRevisionOperatingModeRepository.AddWithoutSave(lineRevisionOperatingMode);
            return lineRevisionOperatingMode;
        }

        public async Task<LineRevisionOperatingMode> Update(LineRevisionOperatingMode lineRevisionOperatingMode)
        {
            // Ensure there isn't a duplicate name (excluding the current record)
            if (_lineRevisionOperatingModeRepository.Search(c => c.LineRevision == lineRevisionOperatingMode.LineRevision && c.Id != lineRevisionOperatingMode.Id).Result.Any())
                return null;

            await _lineRevisionOperatingModeRepository.Update(lineRevisionOperatingMode);
            return lineRevisionOperatingMode;
        }

        public async Task<bool> Remove(LineRevisionOperatingMode lineRevisionOperatingMode)
        {
            await _lineRevisionOperatingModeRepository.Remove(lineRevisionOperatingMode);
            return true;
        }

        //public async Task<IEnumerable<LineRevisionOperatingMode>> Search(string searchCriteria)
        //{
        //    return await _lineRevisionOperatingModeRepository.Search(c => c.Id.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _lineRevisionOperatingModeRepository?.Dispose();
        }
    }
}