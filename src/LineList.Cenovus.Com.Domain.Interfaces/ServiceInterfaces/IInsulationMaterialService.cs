using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IInsulationMaterialService
    {
        Task<IEnumerable<InsulationMaterial>> GetAll();

        Task<InsulationMaterial> GetById(Guid id);

        Task<InsulationMaterial> Add(InsulationMaterial insulationMaterial);

        Task<InsulationMaterial> Update(InsulationMaterial insulationMaterial);

        Task<bool> Remove(InsulationMaterial insulationMaterial);

        Task<IEnumerable<InsulationMaterial>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}