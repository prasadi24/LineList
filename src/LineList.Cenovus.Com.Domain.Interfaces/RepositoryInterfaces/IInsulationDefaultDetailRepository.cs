using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IInsulationDefaultDetailRepository : IRepository<InsulationDefaultDetail>
    {
        new Task<List<InsulationDefaultDetail>> GetAll();

        new Task<InsulationDefaultDetail> GetById(Guid id);

        Task<IEnumerable<InsulationDefaultDetail>> GetByInsulationDefaultId(Guid rowId, Guid ColumnId);
    }
}