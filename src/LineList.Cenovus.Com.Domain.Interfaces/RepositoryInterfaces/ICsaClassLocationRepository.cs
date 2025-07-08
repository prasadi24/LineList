using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ICsaClassLocationRepository : IRepository<CsaClassLocation>
    {
        new Task<List<CsaClassLocation>> GetAll();

        new Task<CsaClassLocation> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}