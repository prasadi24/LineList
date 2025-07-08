using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportSheetColumnService
    {
        Task<IEnumerable<ImportSheetColumn>> GetAll();

        Task<ImportSheetColumn> GetById(Guid id);

        Task<ImportSheetColumn> Add(ImportSheetColumn importSheetColumn);

        Task<ImportSheetColumn> Update(ImportSheetColumn importSheetColumn);

        Task<bool> Remove(ImportSheetColumn importSheetColumn);

        //Task<IEnumerable<ImportSheetColumn>> Search(string searchCriteria);
    }
}