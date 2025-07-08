using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class RoleUserService : IRoleUserService
    {
        private readonly IRoleUserRepository _roleUserRepository;

        public RoleUserService(IRoleUserRepository roleUserRepository)
        {
            _roleUserRepository = roleUserRepository;
        }

        public async Task<IEnumerable<RoleUser>> GetAll()
        {
            return await _roleUserRepository.GetAll();
        }

        public async Task<RoleUser> GetById(Guid id)
        {
            return await _roleUserRepository.GetById(id);
        }

        public async Task<RoleUser> Add(RoleUser roleUser)
        {
            if (_roleUserRepository.Search(c => c.UserName == roleUser.UserName).Result.Any())
                return null;

            await _roleUserRepository.Add(roleUser);
            return roleUser;
        }

        public async Task<RoleUser> Update(RoleUser roleUser)
        {
            if (_roleUserRepository.Search(c => c.UserName == roleUser.UserName && c.Id != roleUser.Id).Result.Any())
                return null;

            await _roleUserRepository.Update(roleUser);
            return roleUser;
        }

        public async Task<bool> Remove(RoleUser roleUser)
        {
            await _roleUserRepository.Remove(roleUser);
            return true;
        }

        public async Task<IEnumerable<RoleUser>> Search(string searchCriteria)
        {
            return await _roleUserRepository.Search(c => c.UserName.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _roleUserRepository?.Dispose();
        }
    }
}