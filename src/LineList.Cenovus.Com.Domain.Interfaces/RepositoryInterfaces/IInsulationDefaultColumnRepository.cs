using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IInsulationDefaultColumnRepository : IRepository<InsulationDefaultColumn>
    {
        new Task<List<InsulationDefaultColumn>> GetAll();

        new Task<InsulationDefaultColumn> GetById(Guid id);

        Task<IEnumerable<InsulationDefaultColumn>> GetByInsulationDefaultId(Guid id);
    }
}