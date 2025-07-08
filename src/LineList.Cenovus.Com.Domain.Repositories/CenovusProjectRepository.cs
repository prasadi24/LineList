using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class CenovusProjectRepository : Repository<CenovusProject>, ICenovusProjectRepository
    {
        private readonly LineListDbContext _context;

        public CenovusProjectRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<CenovusProject>> GetAll()
        {
            return await Db.CenovusProjects.AsNoTracking()
                .Include(b => b.Facility)
                .Include(b => b.ProjectType)
                .ToListAsync();
        }

        public bool HasDependencies(Guid id)
        {
            return _context.EpProjects.Any(m => m.CenovusProjectId == id);
        }
    }
}