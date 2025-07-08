using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILocationTypeService
    {
        Task<IEnumerable<LocationType>> GetAll();

        Task<LocationType> GetById(Guid id);

        Task<LocationType> Add(LocationType locationType);

        Task<LocationType> Update(LocationType locationType);

        Task<bool> Remove(LocationType locationType);

        Task<IEnumerable<LocationType>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}