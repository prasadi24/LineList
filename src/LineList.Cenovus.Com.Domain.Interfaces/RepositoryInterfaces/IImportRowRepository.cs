using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportRowRepository : IRepository<ImportRow>
    {
        new Task<List<ImportRow>> GetAll();

        new Task<ImportRow> GetById(Guid id);
    }
}