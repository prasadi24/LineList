using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IInsulationDefaultDetailService
    {
        Task<IEnumerable<InsulationDefaultDetail>> GetAll();

        Task<InsulationDefaultDetail> GetById(Guid id);

        Task<InsulationDefaultDetail> Add(InsulationDefaultDetail insulationDefaultDetail);

        Task<InsulationDefaultDetail> Update(InsulationDefaultDetail insulationDefaultDetail);

        Task<bool> Remove(InsulationDefaultDetail insulationDefaultDetail);

        Task<IEnumerable<InsulationDefaultDetail>> GetByInsulationDefaultId(Guid rowId, Guid ColumnId);
    }
}