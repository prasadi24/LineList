using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpProjectInsulationDefaultService
    {
        Task<IEnumerable<EpProjectInsulationDefault>> GetAll();

        Task<EpProjectInsulationDefault> GetById(Guid id);

        Task<EpProjectInsulationDefault> Add(EpProjectInsulationDefault epProjectInsulationDefault);

        Task<EpProjectInsulationDefault> Update(EpProjectInsulationDefault epProjectInsulationDefault);

        Task<bool> Remove(EpProjectInsulationDefault epProjectInsulationDefault);

        Task<IEnumerable<EpProjectInsulationDefault>> Search(string searchCriteria);
    }
}