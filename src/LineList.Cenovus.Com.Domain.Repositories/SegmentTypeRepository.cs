using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class SegmentTypeRepository : Repository<SegmentType>, ISegmentTypeRepository
{
    public SegmentTypeRepository(LineListDbContext context) : base(context)
    {
    }

    public bool HasDependencies(Guid id)
    {
        return Db.LineRevisionSegments.Any(m => m.SegmentTypeId == id);
    }
}