using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class CsaClassLocationRepository : Repository<CsaClassLocation>, ICsaClassLocationRepository
    {
        private readonly LineListDbContext _context;

        public CsaClassLocationRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.LineRevisionOperatingModes.Any(m => m.CsaClassLocationId == id);
        }
        
    }
}