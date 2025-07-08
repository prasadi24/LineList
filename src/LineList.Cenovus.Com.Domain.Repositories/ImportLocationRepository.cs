using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ImportLocationRepository : Repository<ImportLocation>, IImportLocationRepository
    {
        private readonly LineListDbContext _context;

        public ImportLocationRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        // Add custom methods if necessary
    }
}