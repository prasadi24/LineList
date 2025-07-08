using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IInsulationMaterialRepository : IRepository<InsulationMaterial>
    {
        new Task<List<InsulationMaterial>> GetAll();

        new Task<InsulationMaterial> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}