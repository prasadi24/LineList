using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpProjectInsulationDefaultDetailRepository : IRepository<EpProjectInsulationDefaultDetail>
    {
        new Task<List<EpProjectInsulationDefaultDetail>> GetAll();

        new Task<EpProjectInsulationDefaultDetail> GetById(Guid id);
        new Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetByInsulationDefaultId(Guid id);
        new Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetByInsulationDefaultId(Guid rowId, Guid ColumnId);
    }
}