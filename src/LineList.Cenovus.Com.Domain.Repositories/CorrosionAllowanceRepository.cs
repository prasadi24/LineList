using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class CorrosionAllowanceRepository : Repository<CorrosionAllowance>, ICorrosionAllowanceRepository
    {
        private readonly LineListDbContext _context;

        public CorrosionAllowanceRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.PipeSpecifications.Any(m => m.CorrosionAllowanceId == id)
                 || _context.LineRevisions.Any(m => m.CorrosionAllowanceAnnulusId == id)
                 || _context.LineRevisions.Any(m => m.CorrosionAllowancePipeId == id);
        }

       
    }
}