using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class InsulationMaterialService : IInsulationMaterialService
    {
        private readonly IInsulationMaterialRepository _insulationMaterialRepository;

        public InsulationMaterialService(IInsulationMaterialRepository insulationMaterialRepository)
        {
            _insulationMaterialRepository = insulationMaterialRepository;
        }

        public async Task<IEnumerable<InsulationMaterial>> GetAll()
        {
            return await _insulationMaterialRepository.GetAll();
        }

        public async Task<InsulationMaterial> GetById(Guid id)
        {
            return await _insulationMaterialRepository.GetById(id);
        }

        public async Task<InsulationMaterial> Add(InsulationMaterial insulationMaterial)
        {
            if (_insulationMaterialRepository.Search(c => c.Name == insulationMaterial.Name).Result.Any())
                return null;

            await _insulationMaterialRepository.Add(insulationMaterial);
            return insulationMaterial;
        }

        public async Task<InsulationMaterial> Update(InsulationMaterial insulationMaterial)
        {
            if (_insulationMaterialRepository.Search(c => c.Name == insulationMaterial.Name && c.Id != insulationMaterial.Id).Result.Any())
                return null;

            await _insulationMaterialRepository.Update(insulationMaterial);
            return insulationMaterial;
        }

        public async Task<bool> Remove(InsulationMaterial insulationMaterial)
        {
            await _insulationMaterialRepository.Remove(insulationMaterial);
            return true;
        }

        public async Task<IEnumerable<InsulationMaterial>> Search(string searchCriteria)
        {
            return await _insulationMaterialRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _insulationMaterialRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _insulationMaterialRepository.HasDependencies(id);
        }
    }
}