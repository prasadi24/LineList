using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpProjectInsulationDefaultRowRepository : Repository<EpProjectInsulationDefaultRow>, IEpProjectInsulationDefaultRowRepository
    {
        private readonly LineListDbContext _context;

        public EpProjectInsulationDefaultRowRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultRow>> GetByInsulationDefaultId(Guid id)
        {
            return await _context.EpProjectInsulationDefaultRows.Where(d => d.EpProjectInsulationDefaultId == id)
                .Include(b => b.SizeNps)
                .ToListAsync();
        }

        // Add custom methods if needed
    }
}