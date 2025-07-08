using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class XrayRepository : Repository<Xray>, IXrayRepository
    {
        private readonly LineListDbContext _context;

        public XrayRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public bool HasDependencies(Guid id)
        {
            return _context.NdeCategories.Any(m => m.XrayId == id)
                || _context.PipeSpecifications.Any(m => m.XrayId == id)
               || _context.LineRevisions.Any(m => m.XrayAnnulusId == id)
               || _context.LineRevisions.Any(m => m.XrayPipeId == id);
        }
    }
}