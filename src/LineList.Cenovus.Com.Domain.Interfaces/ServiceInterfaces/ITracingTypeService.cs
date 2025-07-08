using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ITracingTypeService
    {
        Task<IEnumerable<TracingType>> GetAll();

        Task<TracingType> GetById(Guid id);

        Task<TracingType> Add(TracingType tracingType);

        Task<TracingType> Update(TracingType tracingType);

        Task<bool> Remove(TracingType tracingType);

        Task<IEnumerable<TracingType>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}