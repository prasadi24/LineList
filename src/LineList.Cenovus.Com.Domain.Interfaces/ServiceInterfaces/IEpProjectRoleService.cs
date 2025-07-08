using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpProjectRoleService
    {
        Task<IEnumerable<EpProjectRole>> GetAll();

        Task<EpProjectRole> GetById(Guid id);

        Task<EpProjectRole> Add(EpProjectRole epProjectRole);

        Task<EpProjectRole> Update(EpProjectRole epProjectRole);

        Task<bool> Remove(EpProjectRole epProjectRole);

        Task<IEnumerable<EpProjectRole>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}