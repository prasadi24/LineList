using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IInsulationDefaultRepository : IRepository<InsulationDefault>
    {
        new Task<List<InsulationDefault>> GetAll();

        new Task<InsulationDefault> GetById(Guid id);
    }
}