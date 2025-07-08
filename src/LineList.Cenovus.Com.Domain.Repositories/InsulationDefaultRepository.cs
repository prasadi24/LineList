using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class InsulationDefaultRepository : Repository<InsulationDefault>, IInsulationDefaultRepository
    {
        private readonly LineListDbContext _context;

        public InsulationDefaultRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<InsulationDefault> GetById(Guid id)
        {
            return await _context.InsulationDefaults
                            .Include(b => b.InsulationMaterial)
                            .Include(b => b.InsulationType)
                            .Include(b => b.TracingType)
                             .Where(d => d.Id == id)
                             .FirstOrDefaultAsync();
        }
    }
}