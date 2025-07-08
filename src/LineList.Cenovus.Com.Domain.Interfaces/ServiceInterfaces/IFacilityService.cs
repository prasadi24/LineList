using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IFacilityService
    {
        Task<IEnumerable<Facility>> GetAll();

        Task<Facility> GetById(Guid id);

        Task<Facility> Add(Facility facility);

        Task<Facility> Update(Facility facility);

        Task<bool> Remove(Facility facility);

        Task<IEnumerable<Facility>> Search(string searchCriteria);

        EpCompanyAlpha GetCompanyAlphaForEPCompany(Guid epCompanyID, Guid facilityId);

        bool HasDependencies(Guid facilityId);
    }
}