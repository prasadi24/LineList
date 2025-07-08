using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class OperatingModeRepository : Repository<OperatingMode>, IOperatingModeRepository
    {
        private readonly LineListDbContext _context;

        public OperatingModeRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<OperatingMode?> GetOperatingModeForPrimary()
        {
            return await Db.OperatingModes
                .SingleOrDefaultAsync(x => x.Description == "PRIMARY");
        }

        public bool HasDependencies(Guid id)
        {
            return _context.LineRevisionOperatingModes.Any(m => m.OperatingModeId == id);
        }
    }
}