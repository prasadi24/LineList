using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IFluidPhaseRepository : IRepository<FluidPhase>
    {
        new Task<List<FluidPhase>> GetAll();

        new Task<FluidPhase> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}