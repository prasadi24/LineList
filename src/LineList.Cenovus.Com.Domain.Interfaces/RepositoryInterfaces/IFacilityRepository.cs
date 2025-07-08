using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        new Task<List<Facility>> GetAll();

        new Task<Facility> GetById(Guid id);

        EpCompanyAlpha GetCompanyAlphaForEPCompany(Guid epCompanyID, Guid facilityId);

        bool HasDependencies(Guid facilityId);
    }
}