using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpCompanyAlphaRepository : Repository<EpCompanyAlpha>, IEpCompanyAlphaRepository
    {
        private readonly LineListDbContext _context;

        public EpCompanyAlphaRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<EpCompanyAlpha>> GetAll()
        {
            return await Db.EpCompanyAlphas.AsNoTracking()
                .Include(b => b.Facility)
                .ToListAsync();
        }
        // You can add any custom methods here if required
    }
}