using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class FacilityRepository : Repository<Facility>, IFacilityRepository
    {
        private readonly LineListDbContext _context;

        public FacilityRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public EpCompanyAlpha GetCompanyAlphaForEPCompany(Guid epCompanyID, Guid facilityId)
        {
            return _context.EpCompanyAlphas.Single(x => x.FacilityId == facilityId && x.EpCompanyId == epCompanyID);
        }

        public bool HasDependencies(Guid facilityId)
        {
            return _context.Locations.Any(l => l.FacilityId == facilityId)
                || _context.CenovusProjects.Any(m => m.FacilityId == facilityId)
                || _context.NotesConfigurations.Any(m => m.FacilityId == facilityId)
                || _context.EpCompanyAlphas.Any(m => m.FacilityId == facilityId);
        }
    }
}