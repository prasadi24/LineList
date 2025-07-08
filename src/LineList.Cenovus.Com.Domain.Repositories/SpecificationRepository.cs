using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class SpecificationRepository : Repository<Specification>, ISpecificationRepository
    {
        private readonly LineListDbContext _context;

        public SpecificationRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Specification>> GetAll()
        {
            return await Db.Specifications.AsNoTracking()
                .Include(b => b.Areas)
                 .ThenInclude(a => a.Location)
                .ToListAsync();
        }

        public bool HasDependencies(Guid id)
        {
            return (_context.TracingTypes.Any(m => m.SpecificationId == id)
                || _context.LineListRevisions.Any(m => m.SpecificationId == id)
                || _context.Areas.Any(m => m.SpecificationId == id)
                || _context.PipeSpecifications.Any(m => m.SpecificationId == id)
                || _context.LineRevisions.Any(m => m.SpecificationId == id)
                || _context.Commodities.Any(m => m.SpecificationId == id));
        }
    }
}