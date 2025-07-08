using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class FluidService : IFluidService
    {
        private readonly IFluidRepository _fluidRepository;

        public FluidService(IFluidRepository fluidRepository)
        {
            _fluidRepository = fluidRepository;
        }

        public async Task<IEnumerable<Fluid>> GetAll()
        {
            return await _fluidRepository.GetAll();
        }

        public async Task<Fluid> GetById(Guid id)
        {
            return await _fluidRepository.GetById(id);
        }

        public async Task<Fluid> Add(Fluid fluid)
        {
            // Example check for duplicate name
            if (_fluidRepository.Search(c => c.Name == fluid.Name).Result.Any())
                return null;

            await _fluidRepository.Add(fluid);
            return fluid;
        }

        public async Task<Fluid> Update(Fluid fluid)
        {
            // Example check for duplicate name while updating
            if (_fluidRepository.Search(c => c.Name == fluid.Name && c.Id != fluid.Id).Result.Any())
                return null;

            await _fluidRepository.Update(fluid);
            return fluid;
        }

        public async Task<bool> Remove(Fluid fluid)
        {
            await _fluidRepository.Remove(fluid);
            return true;
        }

        public async Task<IEnumerable<Fluid>> Search(string searchCriteria)
        {
            return await _fluidRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _fluidRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _fluidRepository.HasDependencies(id);
        }
    }
}