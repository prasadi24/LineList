using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILineStatusRepository : IRepository<LineStatus>
    {
        new Task<List<LineStatus>> GetAll();

        new Task<LineStatus> GetById(Guid id);
        Task<Guid> GetStatusIdByName(string name);
        Task<Guid> GetDeletedStatusId();

        bool HasDependencies(Guid id);
    }
}