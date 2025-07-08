using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpProjectRoleRepository : IRepository<EpProjectRole>
    {
        new Task<List<EpProjectRole>> GetAll();

        new Task<EpProjectRole> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}