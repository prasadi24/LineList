using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface INdeCategoryRepository : IRepository<NdeCategory>
    {
        new Task<List<NdeCategory>> GetAll();

        new Task<NdeCategory> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}