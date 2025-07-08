using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITracingDesignNumberOfTracersRepository : IRepository<TracingDesignNumberOfTracers>
    {
        new Task<List<TracingDesignNumberOfTracers>> GetAll();

        new Task<TracingDesignNumberOfTracers> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}