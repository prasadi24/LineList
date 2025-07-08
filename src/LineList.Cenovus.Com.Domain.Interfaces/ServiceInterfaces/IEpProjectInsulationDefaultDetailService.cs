using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpProjectInsulationDefaultDetailService
    {
        Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetAll();

        Task<EpProjectInsulationDefaultDetail> GetById(Guid id);

        Task<EpProjectInsulationDefaultDetail> Add(EpProjectInsulationDefaultDetail epProjectInsulationDefaultDetail);

        Task<EpProjectInsulationDefaultDetail> Update(EpProjectInsulationDefaultDetail epProjectInsulationDefaultDetail);

        Task<bool> Remove(EpProjectInsulationDefaultDetail epProjectInsulationDefaultDetail);

        Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetByInsulationDefaultId(Guid id);
        Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetByInsulationDefaultId(Guid rowId, Guid ColumnId);
    }
}