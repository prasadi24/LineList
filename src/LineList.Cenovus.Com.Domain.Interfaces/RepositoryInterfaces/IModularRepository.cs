using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IModularRepository : IRepository<Modular>
    {
        new Task<List<Modular>> GetAll();

        new Task<Modular> GetById(Guid id);

        Task<List<ModularDropDownModel>> GetModularIds(Guid facilityId, Guid projectTypeId, Guid epCompanyId, Guid epProjectId, Guid cenovusProjectId);
    }
}