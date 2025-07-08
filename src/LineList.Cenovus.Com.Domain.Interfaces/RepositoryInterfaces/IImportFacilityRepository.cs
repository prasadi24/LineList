using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportFacilityRepository : IRepository<ImportFacility>
    {
        new Task<List<ImportFacility>> GetAll();

        new Task<ImportFacility> GetById(Guid id);
    }
}