using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpProjectInsulationDefaultRowService
    {
        Task<IEnumerable<EpProjectInsulationDefaultRow>> GetAll();

        Task<EpProjectInsulationDefaultRow> GetById(Guid id);

        Task<IEnumerable<EpProjectInsulationDefaultRow>> GetByInsulationDefaultId(Guid id);

        Task<EpProjectInsulationDefaultRow> Add(EpProjectInsulationDefaultRow epProjectInsulationDefaultRow);

        Task<EpProjectInsulationDefaultRow> Update(EpProjectInsulationDefaultRow epProjectInsulationDefaultRow);

        Task<bool> Remove(EpProjectInsulationDefaultRow epProjectInsulationDefaultRow);

        //Task<IEnumerable<EpProjectInsulationDefaultRow>> Search(string searchCriteria);
    }
}