using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IInsulationDefaultRowService
    {
        Task<IEnumerable<InsulationDefaultRow>> GetAll();

        Task<InsulationDefaultRow> GetById(Guid id);

        Task<IEnumerable<InsulationDefaultRow>> GetByInsulationDefaultId(Guid id);

        Task<InsulationDefaultRow> Add(InsulationDefaultRow insulationDefaultRow);

        Task<InsulationDefaultRow> Update(InsulationDefaultRow insulationDefaultRow);

        Task<bool> Remove(InsulationDefaultRow insulationDefaultRow);

        //Task<IEnumerable<InsulationDefaultRow>> Search(string searchCriteria);
    }
}