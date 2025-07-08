using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LocationTypeRepository : Repository<LocationType>, ILocationTypeRepository
    {
        private readonly LineListDbContext _context;

        public LocationTypeRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.Locations.Any(p => p.LocationTypeId == id);
        }
    }
}