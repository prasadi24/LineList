using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpProjectInsulationDefaultDetailRepository : Repository<EpProjectInsulationDefaultDetail>, IEpProjectInsulationDefaultDetailRepository
    {
        private readonly LineListDbContext _context;

        public EpProjectInsulationDefaultDetailRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetByInsulationDefaultId(Guid id)
        {
            return await _context.EpProjectInsulationDefaultDetails.Where(d => d.EpProjectInsulationDefaultColumn.EpProjectInsulationDefaultId == id).ToListAsync();
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetByInsulationDefaultId(Guid rowId, Guid ColumnId)
        {
            return await _context.EpProjectInsulationDefaultDetails
                            .Where(d => d.EpProjectInsulationDefaultRowId == rowId && d.EpProjectInsulationDefaultColumnId == ColumnId)
                            .Include(b => b.EpProjectInsulationDefaultRow)
                            .Include(b => b.EpProjectInsulationDefaultColumn)
                            .Include(b => b.InsulationThickness)
                            .Include(b => b.TracingDesignNumberOfTracers)
                            .ToListAsync();
        }
        // Add custom methods if needed
    }
}