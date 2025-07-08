using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class ModularService : IModularService
    {
        private readonly IModularRepository _modularRepository;

        public ModularService(IModularRepository modularRepository)
        {
            _modularRepository = modularRepository;
        }

        public async Task<IEnumerable<Modular>> GetAll()
        {
            return await _modularRepository.GetAll();
        }

        public async Task<Modular> GetById(Guid id)
        {
            return await _modularRepository.GetById(id);
        }

        public async Task<Modular> Add(Modular modular)
        {
            // Prevent adding a duplicate Modular entry based on Name
            if (_modularRepository.Search(c => c.Name == modular.Name).Result.Any())
                return null;

            await _modularRepository.Add(modular);
            return modular;
        }

        public async Task<Modular> Update(Modular modular)
        {
            // Prevent updating to a duplicate Modular entry based on Name
            if (_modularRepository.Search(c => c.Name == modular.Name && c.Id != modular.Id).Result.Any())
                return null;

            await _modularRepository.Update(modular);
            return modular;
        }

        public async Task<bool> Remove(Modular modular)
        {
            await _modularRepository.Remove(modular);
            return true;
        }

        public async Task<IEnumerable<Modular>> Search(string searchCriteria)
        {
            return await _modularRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public async Task<List<ModularDropDownModel>> GetModularDetails(Guid facilityId, Guid projectTypeId, Guid epCompanyId, Guid epProjectId, Guid cenovusProjectId)
        {
            return await _modularRepository.GetModularIds(facilityId, projectTypeId, epCompanyId, epProjectId, cenovusProjectId);
        }

        public void Dispose()
        {
            _modularRepository?.Dispose();
        }
    }
}