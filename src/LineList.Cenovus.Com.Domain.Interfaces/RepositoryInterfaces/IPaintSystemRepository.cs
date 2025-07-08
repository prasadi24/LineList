using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IPaintSystemRepository : IRepository<PaintSystem>
    {
        new Task<List<PaintSystem>> GetAll();

        new Task<PaintSystem> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}