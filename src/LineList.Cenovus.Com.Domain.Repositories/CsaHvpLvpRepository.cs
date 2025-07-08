using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class CsaHvpLvpRepository : Repository<CsaHvpLvp>, ICsaHvpLvpRepository
    {
        private readonly LineListDbContext _context;

        public CsaHvpLvpRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.LineRevisionOperatingModes.Any(m => m.CsaHvpLvpId == id);
        }

        
    }
}