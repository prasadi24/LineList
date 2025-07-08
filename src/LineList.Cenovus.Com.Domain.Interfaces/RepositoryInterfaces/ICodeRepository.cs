using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ICodeRepository : IRepository<Code>
    {
        new Task<List<Code>> GetAll();

        new Task<Code> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}