using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ICommodityRepository : IRepository<Commodity>
    {
        new Task<List<Commodity>> GetAll();

        new Task<Commodity> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}