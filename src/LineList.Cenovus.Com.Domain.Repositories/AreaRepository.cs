using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class AreaRepository : Repository<Area>, IAreaRepository
    {
        private readonly LineListDbContext _context;

        public AreaRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Area>> GetAll()
        {
            return await Db.Areas.AsNoTracking()
                .Include(b => b.Location)
                .Include(b => b.Specification)
                .ToListAsync();
        }

        public bool HasDependencies(Guid areaId)
        {
            return (_context.LineListRevisions.Any(l => l.AreaId == areaId)
                || _context.LineRevisions.Any(m => m.AreaId == areaId));               
        }
    }
}