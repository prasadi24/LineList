using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportSheetService
    {
        Task<IEnumerable<ImportSheet>> GetAll();

        Task<ImportSheet> GetById(Guid id);

        Task<ImportSheet> Add(ImportSheet importSheet);

        Task<ImportSheet> Update(ImportSheet importSheet);

        Task<bool> Remove(ImportSheet importSheet);

        Task<IEnumerable<ImportSheet>> Search(string searchCriteria);
    }
}