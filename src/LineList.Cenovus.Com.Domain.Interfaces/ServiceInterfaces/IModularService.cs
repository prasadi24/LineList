using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IModularService
    {
        Task<IEnumerable<Modular>> GetAll();

        Task<Modular> GetById(Guid id);

        Task<Modular> Add(Modular modular);

        Task<Modular> Update(Modular modular);

        Task<bool> Remove(Modular modular);

        Task<List<ModularDropDownModel>> GetModularDetails(Guid facilityId, Guid projectTypeId, Guid epCompanyId, Guid epProjectId, Guid cenovusProjectId);

        Task<IEnumerable<Modular>> Search(string searchCriteria);
    }
}