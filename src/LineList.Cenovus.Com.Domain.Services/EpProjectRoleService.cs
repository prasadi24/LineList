using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpProjectRoleService : IEpProjectRoleService
    {
        private readonly IEpProjectRoleRepository _epProjectRoleRepository;

        public EpProjectRoleService(IEpProjectRoleRepository epProjectRoleRepository)
        {
            _epProjectRoleRepository = epProjectRoleRepository;
        }

        public async Task<IEnumerable<EpProjectRole>> GetAll()
        {
            return await _epProjectRoleRepository.GetAll();
        }

        public async Task<EpProjectRole> GetById(Guid id)
        {
            return await _epProjectRoleRepository.GetById(id);
        }

        public async Task<EpProjectRole> Add(EpProjectRole epProjectRole)
        {
            // Example condition for checking before adding
            if (_epProjectRoleRepository.Search(c => c.Name == epProjectRole.Name).Result.Any())
                return null;

            await _epProjectRoleRepository.Add(epProjectRole);
            return epProjectRole;
        }

        public async Task<EpProjectRole> Update(EpProjectRole epProjectRole)
        {
            // Example condition for checking before updating
            if (_epProjectRoleRepository.Search(c => c.Name == epProjectRole.Name && c.Id != epProjectRole.Id).Result.Any())
                return null;

            await _epProjectRoleRepository.Update(epProjectRole);
            return epProjectRole;
        }

        public async Task<bool> Remove(EpProjectRole epProjectRole)
        {
            await _epProjectRoleRepository.Remove(epProjectRole);
            return true;
        }

        public async Task<IEnumerable<EpProjectRole>> Search(string searchCriteria)
        {
            return await _epProjectRoleRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _epProjectRoleRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _epProjectRoleRepository.HasDependencies(id);
        }
    }
}