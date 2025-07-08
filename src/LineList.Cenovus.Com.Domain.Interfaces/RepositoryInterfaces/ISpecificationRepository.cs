using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ISpecificationRepository : IRepository<Specification>
    {
        new Task<List<Specification>> GetAll();

        new Task<Specification> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}