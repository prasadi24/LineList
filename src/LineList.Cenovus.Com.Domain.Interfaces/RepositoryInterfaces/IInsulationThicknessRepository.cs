using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IInsulationThicknessRepository : IRepository<InsulationThickness>
    {
        new Task<List<InsulationThickness>> GetAll();

        new Task<InsulationThickness> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}