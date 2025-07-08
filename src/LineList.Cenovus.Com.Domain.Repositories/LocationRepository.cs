using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LocationRepository : Repository<Location>, ILocationRepository
    {
        private readonly LineListDbContext _context;

        public LocationRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Location>> GetAll()
        {
            return await Db.Locations.AsNoTracking()
                .Include(b => b.Facility)
                .Include(b => b.LocationType)
                .ToListAsync();
        }

        public bool HasDependencies(Guid id)
        {
            return _context.Areas.Any(l => l.LocationId == id)
               || _context.Lines.Any(m => m.LocationId == id)
               || _context.LineRevisions.Any(m => m.Line.LocationId == id)
               || _context.LineListRevisions.Any(m => m.LocationId == id);
        }
    }
}