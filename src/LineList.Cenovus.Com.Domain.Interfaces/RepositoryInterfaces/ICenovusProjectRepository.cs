using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ICenovusProjectRepository : IRepository<CenovusProject>
    {
        new Task<List<CenovusProject>> GetAll();

        new Task<CenovusProject> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}