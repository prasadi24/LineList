using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IInsulationDefaultService
    {
        Task<IEnumerable<InsulationDefault>> GetAll();

        Task<InsulationDefault> GetById(Guid id);

        Task<InsulationDefault> Add(InsulationDefault insulationDefault);

        Task<InsulationDefault> Update(InsulationDefault insulationDefault);

        Task<bool> Remove(InsulationDefault insulationDefault);

        Task<IEnumerable<InsulationDefault>> Search(string searchCriteria);
    }
}