using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;


namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ConcurrentEngineeringLineRepository : Repository<ConcurrentEngineeringLine>, IConcurrentEngineeringLineRepository
    {
        private readonly LineListDbContext _context;

        public ConcurrentEngineeringLineRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConcurrentEngineeringLine>> GetAllLines()
        {
            return await _context.ConcurrentEngineeringLines.ToListAsync();
        }

        public IQueryable<ConcurrentEngineeringLine> GetAllLinesQuery()
        {
            return _context.ConcurrentEngineeringLines.AsNoTracking();
        }
        // Custom repository methods specific to ConcurrentEngineeringLine can be added here
    }
}