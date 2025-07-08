using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportLocationRepository : IRepository<ImportLocation>
    {
        new Task<List<ImportLocation>> GetAll();

        new Task<ImportLocation> GetById(Guid id);
    }
}