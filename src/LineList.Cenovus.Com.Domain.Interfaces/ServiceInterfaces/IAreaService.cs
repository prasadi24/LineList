using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IAreaService
    {
        Task<IEnumerable<Area>> GetAll();

        Task<Area> GetById(Guid id);

        Task<Area> Add(Area area);

        Task<Area> Update(Area area);

        Task<bool> Remove(Area area);

        Task<IEnumerable<Area>> Search(string searchCriteria);

        bool HasDependencies(Guid areaId);
    }
}