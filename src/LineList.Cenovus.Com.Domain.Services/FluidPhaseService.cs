using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class FluidPhaseService : IFluidPhaseService
    {
        private readonly IFluidPhaseRepository _fluidPhaseRepository;

        public FluidPhaseService(IFluidPhaseRepository fluidPhaseRepository)
        {
            _fluidPhaseRepository = fluidPhaseRepository;
        }

        public async Task<IEnumerable<FluidPhase>> GetAll()
        {
            return await _fluidPhaseRepository.GetAll();
        }

        public async Task<FluidPhase> GetById(Guid id)
        {
            return await _fluidPhaseRepository.GetById(id);
        }

        public async Task<FluidPhase> Add(FluidPhase fluidPhase)
        {
            // Example check for duplicate name
            if (_fluidPhaseRepository.Search(c => c.Name == fluidPhase.Name).Result.Any())
                return null;

            await _fluidPhaseRepository.Add(fluidPhase);
            return fluidPhase;
        }

        public async Task<FluidPhase> Update(FluidPhase fluidPhase)
        {
            // Example check for duplicate name while updating
            if (_fluidPhaseRepository.Search(c => c.Name == fluidPhase.Name && c.Id != fluidPhase.Id).Result.Any())
                return null;

            await _fluidPhaseRepository.Update(fluidPhase);
            return fluidPhase;
        }

        public async Task<bool> Remove(FluidPhase fluidPhase)
        {
            await _fluidPhaseRepository.Remove(fluidPhase);
            return true;
        }

        public async Task<IEnumerable<FluidPhase>> Search(string searchCriteria)
        {
            return await _fluidPhaseRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _fluidPhaseRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _fluidPhaseRepository.HasDependencies(id);
        }
    }
}