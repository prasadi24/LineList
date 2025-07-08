using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILocationService
    {
        Task<IEnumerable<Location>> GetAll();

        Task<Location> GetById(Guid id);

        Task<Location> Add(Location location);

        Task<Location> Update(Location location);

        Task<bool> Remove(Location location);

        Task<IEnumerable<Location>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}