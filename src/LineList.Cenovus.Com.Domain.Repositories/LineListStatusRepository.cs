using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LineListStatusRepository : Repository<LineListStatus>, ILineListStatusRepository
    {
        private readonly LineListDbContext _context;

        public LineListStatusRepository(LineListDbContext context) : base(context)
        {
             _context = context;
        }
        public async Task<Guid?> GetCancelledDraftId()
        {
            return await Db.LineListStatuses
                .Where(s => s.Name.ToUpper() == "CANCELLED DRAFT")
                .Select(s => (Guid?)s.Id)
                .FirstOrDefaultAsync();
        }

        public bool HasDependencies(Guid id)
        {           
            return _context.LineListStatuses.Any(m => m.IsIssuedOfId == id)
                || _context.LineListStatuses.Any(m => m.IsDraftOfId == id)
                || _context.LineListRevisions.Any(m => m.LineListStatusId == id)
                || _context.LineListStatusStates.Any(m => m.RequiredIssuedStatus1Id == id)
                || _context.LineListStatusStates.Any(m => m.RequiredIssuedStatus2Id == id)
                || _context.LineListStatusStates.Any(m => m.RequiredIssuedStatus3Id == id)
                || _context.LineListStatusStates.Any(m => m.CurrentStatusId == id)
                || _context.LineListStatusStates.Any(m => m.FutureStatusId == id)
                || _context.LineListStatusStates.Any(m => m.ExcludeIssuedStatus1Id == id);
        }
    }
}