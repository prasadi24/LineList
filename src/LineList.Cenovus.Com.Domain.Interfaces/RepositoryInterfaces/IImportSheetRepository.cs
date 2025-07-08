using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportSheetRepository : IRepository<ImportSheet>
    {
        new Task<List<ImportSheet>> GetAll();

        new Task<ImportSheet> GetById(Guid id);
    }
}