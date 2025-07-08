using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILocationRepository : IRepository<Location>
    {
        new Task<List<Location>> GetAll();

        new Task<Location> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}