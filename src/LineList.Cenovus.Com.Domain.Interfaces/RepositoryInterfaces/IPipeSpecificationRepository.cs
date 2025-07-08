using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IPipeSpecificationRepository : IRepository<PipeSpecification>
    {
        new Task<List<PipeSpecification>> GetAll();

        new Task<PipeSpecification> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}