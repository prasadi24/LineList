using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpProjectRoleRepository : Repository<EpProjectRole>, IEpProjectRoleRepository
    {
        private readonly LineListDbContext _context;

        public EpProjectRoleRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.EpProjectUserRoles.Any(m => m.EpProjectRoleId == id);
        }

       
    }
}