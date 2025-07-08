using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpProjectInsulationDefaultColumnService
    {
        Task<IEnumerable<EpProjectInsulationDefaultColumn>> GetAll();

        Task<EpProjectInsulationDefaultColumn> GetById(Guid id);
        Task<IEnumerable<EpProjectInsulationDefaultColumn>> GetByInsulationDefaultId(Guid id);

        Task<EpProjectInsulationDefaultColumn> Add(EpProjectInsulationDefaultColumn epProjectInsulationDefaultColumn);

        Task<EpProjectInsulationDefaultColumn> Update(EpProjectInsulationDefaultColumn epProjectInsulationDefaultColumn);

        Task<bool> Remove(EpProjectInsulationDefaultColumn epProjectInsulationDefaultColumn);

        //Task<IEnumerable<EpProjectInsulationDefaultColumn>> Search(string searchCriteria);
    }
}