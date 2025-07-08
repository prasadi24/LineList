using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class ReferenceLineRepository : Repository<ReferenceLine>, IReferenceLineRepository
{
    public ReferenceLineRepository(LineListDbContext context) : base(context)
    {
    }
}