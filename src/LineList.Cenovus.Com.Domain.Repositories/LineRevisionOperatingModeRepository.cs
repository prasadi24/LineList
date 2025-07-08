using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LineRevisionOperatingModeRepository : Repository<LineRevisionOperatingMode>, ILineRevisionOperatingModeRepository
    {
        private readonly LineListDbContext _context;

        public LineRevisionOperatingModeRepository(LineListDbContext context) : base(context)
        {
        }

        public async Task<List<LineRevisionOperatingMode>> GetOperatingModesByLineRevisionId(Guid lineRevisionId)
        {
            return await Db.LineRevisionOperatingModes
                .Where(m => m.LineRevisionId == lineRevisionId)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<LineRevisionOperatingMode> GetPrimaryOperatingMode(Guid lineRevisionId)
        {
            return await Db.LineRevisionOperatingModes
                .Where(m => m.LineRevisionId == lineRevisionId && (m.OperatingModeNumber == "1" || string.IsNullOrEmpty(m.OperatingModeNumber)))
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}