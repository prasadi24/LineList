using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LineRevisionSegmentRepository : Repository<LineRevisionSegment>, ILineRevisionSegmentRepository
    {
        private readonly LineListDbContext _context;

        public LineRevisionSegmentRepository(LineListDbContext context) : base(context)
        {
        }

        public async Task<List<LineRevisionSegment>> GetSegmentsByLineRevisionId(Guid lineRevisionId)
        {
            return await Db.LineRevisionSegments
                .Where(m => m.LineRevisionId == lineRevisionId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<LineRevisionSegment> GetFirstSegment(Guid lineRevisionId)
        {
            return await Db.LineRevisionSegments
                .Where(m => m.LineRevisionId == lineRevisionId && m.SegmentNumber == "1")
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

    }
}