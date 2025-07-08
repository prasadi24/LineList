using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpProjectUserRoleService
    {
        Task<IEnumerable<EpProjectUserRole>> GetAll();

        Task<EpProjectUserRole> GetById(Guid id);

        Task<EpProjectUserRole> Add(EpProjectUserRole epProjectUserRole);

        Task<EpProjectUserRole> Update(EpProjectUserRole epProjectUserRole);

        Task<bool> Remove(EpProjectUserRole epProjectUserRole);
        Task<bool> RemoveWithoutSave(EpProjectUserRole epProjectUserRole);

        Task<bool> SaveChanges();

        Task<IEnumerable<EpProjectUserRole>> Search(string searchCriteria);
        // NEW: Method for role uniqueness check
        Task<bool> UserInRole(Guid epProjectId, string userName, Guid? excludeRoleId = null);
    }
}