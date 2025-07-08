using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class SizeNpsRepository : Repository<SizeNps>, ISizeNpsRepository
    {
        private readonly LineListDbContext _context;

        public SizeNpsRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.ScheduleDefaults.Any(m => m.SizeNpsId == id)
                 || _context.LineRevisions.Any(m => m.SizeNpsAnnulusId == id)
                 || _context.LineRevisions.Any(m => m.SizeNpsPipeId == id);
        }
    }
}