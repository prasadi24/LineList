using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class StressAnalysisRepository : Repository<StressAnalysis>, IStressAnalysisRepository
    {
        private readonly LineListDbContext _context;

        public StressAnalysisRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.LineRevisions.Any(m => m.StressAnalysisId == id);
        }
    }
}