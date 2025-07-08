using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IFluidPhaseService
    {
        Task<IEnumerable<FluidPhase>> GetAll();

        Task<FluidPhase> GetById(Guid id);

        Task<FluidPhase> Add(FluidPhase fluidPhase);

        Task<FluidPhase> Update(FluidPhase fluidPhase);

        Task<bool> Remove(FluidPhase fluidPhase);

        Task<IEnumerable<FluidPhase>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}