using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpProjectInsulationDefaultColumnRepository : IRepository<EpProjectInsulationDefaultColumn>
    {
        new Task<List<EpProjectInsulationDefaultColumn>> GetAll();

        new Task<EpProjectInsulationDefaultColumn> GetById(Guid id);
        new Task<IEnumerable<EpProjectInsulationDefaultColumn>> GetByInsulationDefaultId(Guid id);
    }
}