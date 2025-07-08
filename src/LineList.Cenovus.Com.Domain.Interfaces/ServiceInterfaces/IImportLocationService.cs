using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportLocationService
    {
        Task<IEnumerable<ImportLocation>> GetAll();

        Task<ImportLocation> GetById(Guid id);

        Task<ImportLocation> Add(ImportLocation importLocation);

        Task<ImportLocation> Update(ImportLocation importLocation);

        Task<bool> Remove(ImportLocation importLocation);

        //Task<IEnumerable<ImportLocation>> Search(string searchCriteria);
    }
}