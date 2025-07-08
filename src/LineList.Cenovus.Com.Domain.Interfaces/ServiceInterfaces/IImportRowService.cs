using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportRowService
    {
        Task<IEnumerable<ImportRow>> GetAll();

        Task<ImportRow> GetById(Guid id);

        Task<ImportRow> Add(ImportRow importRow);

        Task<ImportRow> Update(ImportRow importRow);

        Task<bool> Remove(ImportRow importRow);

        //Task<IEnumerable<ImportRow>> Search(string searchCriteria);
    }
}