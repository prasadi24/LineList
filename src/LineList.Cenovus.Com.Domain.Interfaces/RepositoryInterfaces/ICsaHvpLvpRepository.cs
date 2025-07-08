using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ICsaHvpLvpRepository : IRepository<CsaHvpLvp>
    {
        new Task<List<CsaHvpLvp>> GetAll();

        new Task<CsaHvpLvp> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}