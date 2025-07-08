using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ITracingDesignNumberOfTracersService
    {
        Task<IEnumerable<TracingDesignNumberOfTracers>> GetAll();

        Task<TracingDesignNumberOfTracers> GetById(Guid id);

        Task<TracingDesignNumberOfTracers> Add(TracingDesignNumberOfTracers tracingDesignNumberOfTracers);

        Task<TracingDesignNumberOfTracers> Update(TracingDesignNumberOfTracers tracingDesignNumberOfTracers);

        Task<bool> Remove(TracingDesignNumberOfTracers tracingDesignNumberOfTracers);

        Task<IEnumerable<TracingDesignNumberOfTracers>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}