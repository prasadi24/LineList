using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILineListStatusRepository : IRepository<LineListStatus>
    {
        new Task<List<LineListStatus>> GetAll();

        new Task<LineListStatus> GetById(Guid id);
        Task<Guid?> GetCancelledDraftId();

        bool HasDependencies(Guid id);
    }
}