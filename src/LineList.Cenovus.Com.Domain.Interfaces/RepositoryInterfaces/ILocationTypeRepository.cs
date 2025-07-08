using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILocationTypeRepository : IRepository<LocationType>
    {
        new Task<List<LocationType>> GetAll();

        new Task<LocationType> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}