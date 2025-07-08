using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITracingTypeRepository : IRepository<TracingType>
    {
        new Task<List<TracingType>> GetAll();

        new Task<TracingType> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}