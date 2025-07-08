using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpProjectInsulationDefaultColumnRepository : Repository<EpProjectInsulationDefaultColumn>, IEpProjectInsulationDefaultColumnRepository
    {
        private readonly LineListDbContext _context;

        public EpProjectInsulationDefaultColumnRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultColumn>> GetByInsulationDefaultId(Guid id)
        {
            return await _context.EpProjectInsulationDefaultColumns.Where(d => d.EpProjectInsulationDefaultId == id).ToListAsync();
        }
        public override async Task<EpProjectInsulationDefaultColumn> GetById(Guid id)
        {
            return await _context.EpProjectInsulationDefaultColumns
                            .Include(b => b.EpProjectInsulationDefault)
                            .ThenInclude(b => b.TracingType)
                             .Where(d => d.Id == id)
                             .FirstOrDefaultAsync();
        }
        // You can add any custom methods here if required
    }
}