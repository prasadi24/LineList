using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ImportRowRepository : Repository<ImportRow>, IImportRowRepository
    {
        private readonly LineListDbContext _context;

        public ImportRowRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        // Add custom methods if necessary
    }
}