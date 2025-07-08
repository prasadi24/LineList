using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IScheduleDefaultRepository : IRepository<ScheduleDefault>
{
    new Task<List<ScheduleDefault>> GetAll();

    new Task<ScheduleDefault> GetById(Guid id);
}