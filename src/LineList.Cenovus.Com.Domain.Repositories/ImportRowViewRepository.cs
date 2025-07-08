using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ImportRowViewRepository : Repository<ImportRowView>, IImportRowViewRepository
    {
        private readonly LineListDbContext _context;

        public ImportRowViewRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }
    }
}