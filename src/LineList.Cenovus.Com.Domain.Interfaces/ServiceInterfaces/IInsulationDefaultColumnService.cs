using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IInsulationDefaultColumnService
    {
        Task<IEnumerable<InsulationDefaultColumn>> GetAll();

        Task<InsulationDefaultColumn> GetById(Guid id);

        Task<IEnumerable<InsulationDefaultColumn>> GetByInsulationDefaultId(Guid id);

        Task<InsulationDefaultColumn> Add(InsulationDefaultColumn insulationDefaultColumn);

        Task<InsulationDefaultColumn> Update(InsulationDefaultColumn insulationDefaultColumn);

        Task<bool> Remove(InsulationDefaultColumn insulationDefaultColumn);

        //Task<IEnumerable<InsulationDefaultColumn>> Search(string searchCriteria);
    }
}