using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IImportFacilityService
    {
        Task<IEnumerable<ImportFacility>> GetAll();

        Task<ImportFacility> GetById(Guid id);

        Task<ImportFacility> Add(ImportFacility importFacility);

        Task<ImportFacility> Update(ImportFacility importFacility);

        Task<bool> Remove(ImportFacility importFacility);

        //Task<IEnumerable<ImportFacility>> Search(string searchCriteria);
    }
}