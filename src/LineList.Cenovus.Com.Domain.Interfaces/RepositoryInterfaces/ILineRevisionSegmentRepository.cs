using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILineRevisionSegmentRepository : IRepository<LineRevisionSegment>
    {
        new Task<List<LineRevisionSegment>> GetAll();

        new Task<LineRevisionSegment> GetById(Guid id);
        Task<List<LineRevisionSegment>> GetSegmentsByLineRevisionId(Guid lineRevisionId);
        Task<LineRevisionSegment> GetFirstSegment(Guid lineRevisionId);
    }
}