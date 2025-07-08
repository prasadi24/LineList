using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class TestMediumRepository : Repository<TestMedium>, ITestMediumRepository
    {
        private readonly LineListDbContext _context;

        public TestMediumRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.LineRevisions.Any(m => m.TestMediumAnnulusId == id)
                || _context.LineRevisions.Any(m => m.TestMediumPipeId == id);
        }
    }
}