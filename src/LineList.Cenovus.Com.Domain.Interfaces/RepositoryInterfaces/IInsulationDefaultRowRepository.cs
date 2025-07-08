using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IInsulationDefaultRowRepository : IRepository<InsulationDefaultRow>
    {
        new Task<List<InsulationDefaultRow>> GetAll();

        new Task<InsulationDefaultRow> GetById(Guid id);

        Task<IEnumerable<InsulationDefaultRow>> GetByInsulationDefaultId(Guid id);
    }
}