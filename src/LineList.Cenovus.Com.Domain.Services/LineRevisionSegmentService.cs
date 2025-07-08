using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LineRevisionSegmentService : ILineRevisionSegmentService
    {
        private readonly ILineRevisionSegmentRepository _lineRevisionSegmentRepository;

        public LineRevisionSegmentService(ILineRevisionSegmentRepository lineRevisionSegmentRepository)
        {
            _lineRevisionSegmentRepository = lineRevisionSegmentRepository;
        }

        public async Task<IEnumerable<LineRevisionSegment>> GetAll()
        {
            return await _lineRevisionSegmentRepository.GetAll();
        }

        public async Task<LineRevisionSegment> GetById(Guid id)
        {
            return await _lineRevisionSegmentRepository.GetById(id);
        }
        public async Task<List<LineRevisionSegment>> GetSegmentsByLineRevisionId(Guid lineRevisionId)
        {
            return await _lineRevisionSegmentRepository.GetSegmentsByLineRevisionId(lineRevisionId);
        }

        public async Task<LineRevisionSegment> GetFirstSegment(Guid lineRevisionId)
        {
            return await _lineRevisionSegmentRepository.GetFirstSegment(lineRevisionId);
        }
        public async Task<LineRevisionSegment> Add(LineRevisionSegment lineRevisionSegment)
        {
            // Check if the LineRevisionSegment already exists by its Name or other unique property
            if (_lineRevisionSegmentRepository.Search(c => c.ModifiedBy == lineRevisionSegment.ModifiedBy).Result.Any())
                return null;

            await _lineRevisionSegmentRepository.Add(lineRevisionSegment);
            return lineRevisionSegment;
        }
        public async Task<LineRevisionSegment> AddWithoutSave(LineRevisionSegment lineRevisionSegment)
        {
            // Check if the LineRevisionSegment already exists by its Name or other unique property
            //if (_lineRevisionSegmentRepository.Search(c => c.ModifiedBy == lineRevisionSegment.ModifiedBy).Result.Any())
            //    return null;

            await _lineRevisionSegmentRepository.AddWithoutSave(lineRevisionSegment);
            return lineRevisionSegment;
        }

        public async Task<LineRevisionSegment> Update(LineRevisionSegment lineRevisionSegment)
        {
            // Check for duplicates or other custom logic, if necessary
            if (_lineRevisionSegmentRepository.Search(c => c.ModifiedBy == lineRevisionSegment.ModifiedBy && c.Id != lineRevisionSegment.Id).Result.Any())
                return null;

            await _lineRevisionSegmentRepository.Update(lineRevisionSegment);
            return lineRevisionSegment;
        }

        public async Task<bool> Remove(LineRevisionSegment lineRevisionSegment)
        {
            await _lineRevisionSegmentRepository.Remove(lineRevisionSegment);
            return true;
        }

        public async Task<IEnumerable<LineRevisionSegment>> Search(string searchCriteria)
        {
            return await _lineRevisionSegmentRepository.Search(c => c.ModifiedBy.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _lineRevisionSegmentRepository?.Dispose();
        }
    }
}