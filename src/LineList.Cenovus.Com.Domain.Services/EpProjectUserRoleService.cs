using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpProjectUserRoleService : IEpProjectUserRoleService
    {
        private readonly IEpProjectUserRoleRepository _epProjectUserRoleRepository;

        public EpProjectUserRoleService(IEpProjectUserRoleRepository epProjectUserRoleRepository)
        {
            _epProjectUserRoleRepository = epProjectUserRoleRepository;
        }

        public async Task<IEnumerable<EpProjectUserRole>> GetAll()
        {
            return await _epProjectUserRoleRepository.GetAll();
        }

        public async Task<EpProjectUserRole> GetById(Guid id)
        {
            return await _epProjectUserRoleRepository.GetById(id);
        }

        public async Task<EpProjectUserRole> Add(EpProjectUserRole epProjectUserRole)
        {
            await _epProjectUserRoleRepository.Add(epProjectUserRole);
            return epProjectUserRole;
        }

        public async Task<EpProjectUserRole> Update(EpProjectUserRole epProjectUserRole)
        {
            await _epProjectUserRoleRepository.Update(epProjectUserRole);
            return epProjectUserRole;
        }

        public async Task<bool> Remove(EpProjectUserRole epProjectUserRole)
        {
            await _epProjectUserRoleRepository.Remove(epProjectUserRole);
            return true;
        }

        public async Task<bool> RemoveWithoutSave(EpProjectUserRole epProjectUserRole)
        {
            await _epProjectUserRoleRepository.RemoveWithoutSave(epProjectUserRole);
            return true;
        }

        public async Task<bool> SaveChanges()
        {
            await _epProjectUserRoleRepository.SaveChanges();
            return true;
        }

        public async Task<IEnumerable<EpProjectUserRole>> Search(string searchCriteria)
        {
            return await _epProjectUserRoleRepository.Search(c => c.UserName.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _epProjectUserRoleRepository?.Dispose();
        }

        public async Task<bool> UserInRole(Guid epProjectId, string userName, Guid? excludeRoleId = null)
        {
            var roles = await _epProjectUserRoleRepository.GetAll();
            return roles.Any(r => r.EpProjectId == epProjectId && r.UserName == userName && (!excludeRoleId.HasValue || r.Id != excludeRoleId.Value));
        }
    }
}