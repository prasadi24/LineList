using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LineStatusRepository : Repository<LineStatus>, ILineStatusRepository
    {
        private readonly LineListDbContext _context;

        public LineStatusRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Guid> GetStatusIdByName(string name)
        {
            return await Db.LineStatuses
                .Where(x => x.Name.ToUpper() == name.ToUpper())
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
        }
        public async Task<Guid> GetDeletedStatusId()
        {
            return await Db.LineStatuses
                .Where(s => s.Name.ToUpper() == "DELETED")
                .Select(s => s.Id)
                .FirstOrDefaultAsync();
        }

        public bool HasDependencies(Guid id)
        {
            return _context.LineListStatuses.Any(m => m.CorrespondingLineStatusId == id)
                || _context.LineRevisions.Any(m => m.LineStatusId == id);
        }
    }
}