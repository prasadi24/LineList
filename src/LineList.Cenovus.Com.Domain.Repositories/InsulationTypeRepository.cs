using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class InsulationTypeRepository : Repository<InsulationType>, IInsulationTypeRepository
{
    private readonly LineListDbContext _context;

    public InsulationTypeRepository(LineListDbContext context) : base(context)
    {
    }

    public bool HasDependencies(Guid id)
    {
        return Db.InsulationDefaults.Any(p => p.InsulationTypeId == id)
            || Db.LineRevisionSegments.Any(m => m.InsulationTypeId == id);
    }
}