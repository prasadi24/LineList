using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class InternalCoatingLinerRepository : Repository<InternalCoatingLiner>, IInternalCoatingLinerRepository
{
    private readonly LineListDbContext _context;

    public InternalCoatingLinerRepository(LineListDbContext context) : base(context)
    {
        _context = context;
    }

    public bool HasDependencies(Guid id)
    {
        return _context.LineRevisions.Any(m => m.InternalCoatingLinerId == id);
    }
}