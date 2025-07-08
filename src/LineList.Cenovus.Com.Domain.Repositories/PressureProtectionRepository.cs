using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class PressureProtectionRepository : Repository<PressureProtection>, IPressureProtectionRepository
{
    public PressureProtectionRepository(LineListDbContext context) : base(context)
    {
    }

    public bool HasDependencies(Guid id)
    {
        return Db.LineRevisionOperatingModes.Any(m => m.pressureProtectionId == id);
    }
}