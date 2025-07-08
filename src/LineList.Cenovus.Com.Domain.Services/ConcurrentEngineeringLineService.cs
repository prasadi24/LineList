using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ConcurrentEngineeringLineService : IConcurrentEngineeringLineService
    {
        private readonly IConcurrentEngineeringLineRepository _concurrentEngineeringLineRepository;

        public ConcurrentEngineeringLineService(IConcurrentEngineeringLineRepository concurrentEngineeringLineRepository)
        {
            _concurrentEngineeringLineRepository = concurrentEngineeringLineRepository;
        }

        public async Task<IEnumerable<ConcurrentEngineeringLine>> GetAll()
        {
            return await _concurrentEngineeringLineRepository.GetAll();
        }

        public async Task<ConcurrentEngineeringLine> GetById(Guid id)
        {
            return await _concurrentEngineeringLineRepository.GetById(id);
        }

        public async Task<ConcurrentEngineeringLine> Add(ConcurrentEngineeringLine concurrentEngineeringLine)
        {
            // Additional logic to ensure no duplicate entries based on some criteria (if needed)
            if (_concurrentEngineeringLineRepository.Search(c => c.EpProjectId == concurrentEngineeringLine.EpProjectId).Result.Any())
                return null;

            await _concurrentEngineeringLineRepository.Add(concurrentEngineeringLine);
            return concurrentEngineeringLine;
        }

        public async Task<ConcurrentEngineeringLine> Update(ConcurrentEngineeringLine concurrentEngineeringLine)
        {
            // Ensure no duplicate entries
            if (_concurrentEngineeringLineRepository.Search(c => c.EpProjectId == concurrentEngineeringLine.EpProjectId && c.Id != concurrentEngineeringLine.Id).Result.Any())
                return null;

            await _concurrentEngineeringLineRepository.Update(concurrentEngineeringLine);
            return concurrentEngineeringLine;
        }

        public async Task<bool> Remove(ConcurrentEngineeringLine concurrentEngineeringLine)
        {
            await _concurrentEngineeringLineRepository.Remove(concurrentEngineeringLine);
            return true;
        }

        //public async Task<IEnumerable<ConcurrentEngineeringLine>> Search(string searchCriteria)
        //{
        //    return await _concurrentEngineeringLineRepository.Search(c => c.Name.Contains(searchCriteria));
        //}
        public async Task<IEnumerable<ConcurrentEngineeringLine>> GetFilteredLines(Guid facilityId, Guid projectId, DateTime? ldtFromDate, DateTime? ldtToDate, bool showAsBuilt)
        {
            var query = _concurrentEngineeringLineRepository.GetAllLinesQuery();

            if (facilityId != Guid.Empty)
                query = query.Where(l => l.FacilityId == facilityId);

            if (projectId != Guid.Empty)
                query = query.Where(l => l.EpProjectId == projectId);

            if (ldtFromDate.HasValue && ldtFromDate.Value > DateTime.MinValue)
                query = query.Where(l => l.IssuedOn >= ldtFromDate.Value);

            if (ldtToDate.HasValue && ldtToDate.Value > DateTime.MinValue)
                query = query.Where(l => l.IssuedOn <= ldtToDate.Value);

            if (showAsBuilt)
                query = query.Where(l => l.AsBuiltCount > 0 && l.LineStatus.ToLower().Contains("as built")); // mimic old logic

            return query.ToList();
        }

        //public async Task<IEnumerable<ConcurrentEngineeringLine>> GetConcurrentEngineeringLinesByLineListRevisionId (Guid lineListRevisionId)
        //{
        
        //    var 

        //}
        public void Dispose()
        {
            _concurrentEngineeringLineRepository?.Dispose();
        }
    }
}