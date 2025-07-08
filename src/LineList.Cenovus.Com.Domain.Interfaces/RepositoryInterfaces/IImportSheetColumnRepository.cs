using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportSheetColumnRepository : IRepository<ImportSheetColumn>
    {
        new Task<List<ImportSheetColumn>> GetAll();

        new Task<ImportSheetColumn> GetById(Guid id);
    }
}