using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class InsulationDefaultRowRepository : Repository<InsulationDefaultRow>, IInsulationDefaultRowRepository
    {
        private readonly LineListDbContext _context;

        public InsulationDefaultRowRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InsulationDefaultRow>> GetByInsulationDefaultId(Guid id)
        {
            return await _context.InsulationDefaultRows.Where(d => d.InsulationDefaultId == id)
                .Include(b => b.SizeNps)
                .ToListAsync();
        }
    }
}