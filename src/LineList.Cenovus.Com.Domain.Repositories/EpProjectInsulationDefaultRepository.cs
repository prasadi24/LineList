using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpProjectInsulationDefaultRepository : Repository<EpProjectInsulationDefault>, IEpProjectInsulationDefaultRepository
    {
        private readonly LineListDbContext _context;

        public EpProjectInsulationDefaultRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EpProjectInsulationDefault> GetById(Guid id)
        {
            return await _context.EpProjectInsulationDefaults
                            .Include(b => b.InsulationMaterial)
                            .Include(b => b.InsulationType)
                            .Include(b => b.TracingType)
                             .Where(d => d.Id == id)
                             .FirstOrDefaultAsync();
        }

        // Add custom methods if needed
    }
}