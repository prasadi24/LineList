using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportCommodityService
    {
        Task<IEnumerable<ImportCommodity>> GetAll();

        Task<ImportCommodity> GetById(Guid id);

        Task<ImportCommodity> Add(ImportCommodity importCommodity);

        Task<ImportCommodity> Update(ImportCommodity importCommodity);

        Task<bool> Remove(ImportCommodity importCommodity);

        //Task<IEnumerable<ImportCommodity>> Search(string searchCriteria);
    }
}