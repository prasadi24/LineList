using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class InsulationDefaultDetailRepository : Repository<InsulationDefaultDetail>, IInsulationDefaultDetailRepository
    {
        private readonly LineListDbContext _context;

        public InsulationDefaultDetailRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InsulationDefaultDetail>> GetByInsulationDefaultId(Guid rowId, Guid ColumnId)
        {
            return await _context.InsulationDefaultDetails
                            .Where(d => d.InsulationDefaultRowId == rowId && d.InsulationDefaultColumnId == ColumnId)
                            .Include(b => b.InsulationDefaultRow)
                            .Include(b => b.InsulationDefaultColumn)
                            .Include(b => b.InsulationThickness)
                            .Include(b => b.TracingDesignNumberOfTracers)
                            .ToListAsync();
        }
    }
}