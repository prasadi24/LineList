using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class PipeSpecificationRepository : Repository<PipeSpecification>, IPipeSpecificationRepository
    {
        private readonly LineListDbContext _context;

        public PipeSpecificationRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<PipeSpecification>> GetAll()
        {
            return await Db.PipeSpecifications.AsNoTracking()
                .Include(b => b.Specification)
                .Include(b => b.CorrosionAllowance)
                .Include(b => b.NdeCategory)
                .Include(b => b.Xray)
                .ToListAsync();
        }

        public bool HasDependencies(Guid id)
        {
            return _context.LineRevisions.Any(m => m.PipeSpecificationId == id)
                || _context.ScheduleDefaults.Any(m => m.PipeSpecificationId == id);
        }
    }
}