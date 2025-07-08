using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ImportCommodityRepository : Repository<ImportCommodity>, IImportCommodityRepository
    {
        private readonly LineListDbContext _context;

        public ImportCommodityRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        // Add any custom methods if needed
    }
}