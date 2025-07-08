using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LineListStatusStateRepository : Repository<LineListStatusState>, ILineListStatusStateRepository
    {
        private readonly LineListDbContext _context;

        public LineListStatusStateRepository(LineListDbContext context) : base(context)
        {
        }
    }
}