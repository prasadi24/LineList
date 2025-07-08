using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class InsulationThicknessRepository : Repository<InsulationThickness>, IInsulationThicknessRepository
    {
        private readonly LineListDbContext _context;

        public InsulationThicknessRepository(LineListDbContext context) : base(context)
        {
        }

        public bool HasDependencies(Guid id)
        {
            return Db.InsulationDefaultDetails.Any(m => m.InsulationThicknessId == id)
                || Db.LineRevisionSegments.Any(m => m.InsulationThicknessId == id);
        }

        public override async Task<InsulationThickness> GetById(Guid id)
        {
            return await Db.InsulationThicknesses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
                
        }


        
    }
}