using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpProjectUserRoleRepository : IRepository<EpProjectUserRole>
    {
        new Task<List<EpProjectUserRole>> GetAll();

        new Task<EpProjectUserRole> GetById(Guid id);
    }
}