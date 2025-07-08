using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpProjectInsulationDefaultRowRepository : IRepository<EpProjectInsulationDefaultRow>
    {
        new Task<List<EpProjectInsulationDefaultRow>> GetAll();

        new Task<EpProjectInsulationDefaultRow> GetById(Guid id);
        new Task<IEnumerable<EpProjectInsulationDefaultRow>> GetByInsulationDefaultId(Guid id);
    }
}