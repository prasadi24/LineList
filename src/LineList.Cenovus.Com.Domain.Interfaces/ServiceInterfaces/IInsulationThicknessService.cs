using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IInsulationThicknessService
    {
        Task<IEnumerable<InsulationThickness>> GetAll();

        Task<InsulationThickness> GetById(Guid id);

        Task<InsulationThickness> Add(InsulationThickness insulationThickness);

        Task<InsulationThickness> Update(InsulationThickness insulationThickness);

        Task<bool> Remove(InsulationThickness insulationThickness);

        Task<IEnumerable<InsulationThickness>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}