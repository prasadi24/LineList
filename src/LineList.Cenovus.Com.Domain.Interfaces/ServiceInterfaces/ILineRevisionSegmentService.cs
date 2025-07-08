using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILineRevisionSegmentService
    {
        Task<IEnumerable<LineRevisionSegment>> GetAll();

        Task<LineRevisionSegment> GetById(Guid id);
        Task<List<LineRevisionSegment>> GetSegmentsByLineRevisionId(Guid lineRevisionId);
        Task<LineRevisionSegment> GetFirstSegment(Guid lineRevisionId);

        Task<LineRevisionSegment> Add(LineRevisionSegment lineRevisionSegment);
        Task<LineRevisionSegment> AddWithoutSave(LineRevisionSegment lineRevisionSegment);

        Task<LineRevisionSegment> Update(LineRevisionSegment lineRevisionSegment);

        Task<bool> Remove(LineRevisionSegment lineRevisionSegment);

        Task<IEnumerable<LineRevisionSegment>> Search(string searchCriteria);
    }
}