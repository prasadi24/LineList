using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class TracingTypeRepository : Repository<TracingType>, ITracingTypeRepository
    {
        private readonly LineListDbContext _context;

        public TracingTypeRepository(LineListDbContext context) : base(context)
        {
        }

        public override async Task<List<TracingType>> GetAll()
        {
            return await Db.TracingTypes.AsNoTracking()
                .Include(b => b.Specification)
                .ToListAsync();
        }

        public bool HasDependencies(Guid id)
        {
            return Db.InsulationDefaults.Any(m => m.TracingTypeId == id)
                || Db.LineRevisionSegments.Any(m => m.TracingTypeId == id);
        }
    }
}