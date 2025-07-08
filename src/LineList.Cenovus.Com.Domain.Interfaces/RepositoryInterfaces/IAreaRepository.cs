using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IAreaRepository : IRepository<Area>
    {
        new Task<List<Area>> GetAll();

        new Task<Area> GetById(Guid id);

        bool HasDependencies(Guid areaId);
    }
}