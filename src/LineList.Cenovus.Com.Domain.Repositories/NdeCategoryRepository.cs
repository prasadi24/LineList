using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class NdeCategoryRepository : Repository<NdeCategory>, INdeCategoryRepository
    {
        private readonly LineListDbContext _context;

        public NdeCategoryRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<NdeCategory>> GetAll()
        {
            return await Db.NdeCategories.AsNoTracking()
                .Include(b => b.Xray)
                .ToListAsync();
        }

        public bool HasDependencies(Guid id)
        {
            return _context.PipeSpecifications.Any(m => m.NdeCategoryId == id)
                 || _context.LineRevisions.Any(m => m.NdeCategoryAnnulusId == id)
                 || _context.LineRevisions.Any(m => m.NdeCategoryPipeId == id);
        }
    }
}