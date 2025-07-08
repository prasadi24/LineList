using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class InsulationDefaultColumnRepository : Repository<InsulationDefaultColumn>, IInsulationDefaultColumnRepository
    {
        private readonly LineListDbContext _context;

        public InsulationDefaultColumnRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InsulationDefaultColumn>> GetByInsulationDefaultId(Guid id)
        {
            return await _context.InsulationDefaultColumns.Where(d => d.InsulationDefaultId == id).ToListAsync();
        }
        public override async Task<InsulationDefaultColumn> GetById(Guid id)
        {
            return await _context.InsulationDefaultColumns
                            .Include(b => b.InsulationDefault)
                            .ThenInclude(b => b.TracingType)
                             .Where(d => d.Id == id)
                             .FirstOrDefaultAsync();
        }
    }
}