using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IXrayRepository : IRepository<Xray>
    {
        new Task<List<Xray>> GetAll();

        new Task<Xray> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}