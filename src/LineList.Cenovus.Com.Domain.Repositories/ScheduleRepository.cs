using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
{
    public ScheduleRepository(LineListDbContext context) : base(context)
    {
    }

    public bool HasDependencies(Guid id)
    {
        return Db.ScheduleDefaults.Any(m => m.ScheduleId == id)
            || Db.LineRevisions.Any(m => m.ScheduleAnnulusId == id)
            || Db.LineRevisions.Any(m => m.SchedulePipeId == id);
    }
}