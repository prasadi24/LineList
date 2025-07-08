using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpProjectInsulationDefaultRepository : IRepository<EpProjectInsulationDefault>
    {
        new Task<List<EpProjectInsulationDefault>> GetAll();

        new Task<EpProjectInsulationDefault> GetById(Guid id);
    }
}