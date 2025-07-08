using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class InsulationMaterialRepository : Repository<InsulationMaterial>, IInsulationMaterialRepository
    {
        private readonly LineListDbContext _context;

        public InsulationMaterialRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.InsulationDefaults.Any(m => m.InsulationMaterialId == id)
                 || _context.LineRevisionSegments.Any(m => m.InsulationMaterialId == id);
        }
    }
}