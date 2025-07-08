using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class ScheduleDefaultRepository : Repository<ScheduleDefault>, IScheduleDefaultRepository
{
    public ScheduleDefaultRepository(LineListDbContext context) : base(context)
    {
    }

    public override async Task<List<ScheduleDefault>> GetAll()
    {
        return await Db.ScheduleDefaults.AsNoTracking()
            .Include(b => b.Schedule)
            .Include(b => b.PipeSpecification)
            .Include(b => b.PipeSpecification.Specification)
            .Include(b => b.SizeNps)
            .ToListAsync();
    }

    public override async Task<ScheduleDefault> GetById(Guid id)
    {
        return await Db.ScheduleDefaults
            .AsNoTracking()
            .Include(x => x.Schedule)
            .Include(x => x.SizeNps)
            .Include(x => x.PipeSpecification)
                .ThenInclude(ps => ps.Specification)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
}