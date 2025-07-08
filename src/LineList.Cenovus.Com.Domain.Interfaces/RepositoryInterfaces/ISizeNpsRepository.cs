using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ISizeNpsRepository : IRepository<SizeNps>
    {
        new Task<List<SizeNps>> GetAll();

        new Task<SizeNps> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}