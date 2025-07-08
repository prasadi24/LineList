using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ImportSheetRepository : Repository<ImportSheet>, IImportSheetRepository
    {
        private readonly LineListDbContext _context;

        public ImportSheetRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }
    }
}