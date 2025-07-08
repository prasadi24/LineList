using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class CommodityRepository : Repository<Commodity>, ICommodityRepository
    {
        private readonly LineListDbContext _context;

        public CommodityRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<Commodity>> GetAll()
        {
            return await Db.Commodities.AsNoTracking()
                .Include(b => b.Specification)
                .ToListAsync();
        }

        public bool HasDependencies(Guid id)
        {
            return _context.Lines.Any(p=>p.CommodityId==id)
                || _context.LineRevisions.Any(p=>p.Line.CommodityId==id);
        }
    }
}