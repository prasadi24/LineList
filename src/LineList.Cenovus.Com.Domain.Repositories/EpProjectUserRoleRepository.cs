using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpProjectUserRoleRepository : Repository<EpProjectUserRole>, IEpProjectUserRoleRepository
    {
        private readonly LineListDbContext _context;

        public EpProjectUserRoleRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<EpProjectUserRole>> GetAll()
        {
            return await Db.EpProjectUserRoles.AsNoTracking()
                .Include(b => b.EpProject)
                .Include(b => b.EpProjectRole)
                .ToListAsync();
        }

        public async Task<EpProjectUserRole> GetById(Guid id)
        {
            return await Db.EpProjectUserRoles.AsNoTracking()
               .Include(b => b.EpProject)
               .ThenInclude(a => a.EpCompany)
               .Include(b => b.EpProjectRole)
                            .Where(d => d.Id == id)
                             .FirstOrDefaultAsync();
        }
    }
}