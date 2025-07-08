using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportRowExceptionService
    {
        Task<IEnumerable<ImportRowException>> GetAll();

        Task<ImportRowException> GetById(Guid id);

        Task<ImportRowException> Add(ImportRowException importRowException);

        Task<ImportRowException> Update(ImportRowException importRowException);

        Task<bool> Remove(ImportRowException importRowException);

        //Task<IEnumerable<ImportRowException>> Search(string searchCriteria);
    }
}