using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class PaintSystemRepository : Repository<PaintSystem>, IPaintSystemRepository
    {
        private readonly LineListDbContext _context;

        public PaintSystemRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.LineRevisionSegments.Any(m => m.PaintSystemId == id);
        }
    }
}