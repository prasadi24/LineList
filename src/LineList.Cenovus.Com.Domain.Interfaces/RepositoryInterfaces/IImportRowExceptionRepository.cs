using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportRowExceptionRepository : IRepository<ImportRowException>
    {
        new Task<List<ImportRowException>> GetAll();

        new Task<ImportRowException> GetById(Guid id);
    }
}