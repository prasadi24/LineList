using LineList.Cenovus.Com.Domain.Models;

public interface IScheduleDefaultService
{
    Task<IEnumerable<ScheduleDefault>> GetAll();

    Task<ScheduleDefault> GetById(Guid id);

    Task<ScheduleDefault> Add(ScheduleDefault scheduleDefault);

    Task<ScheduleDefault> Update(ScheduleDefault scheduleDefault);

    Task<bool> Remove(ScheduleDefault scheduleDefault);

    Task<IEnumerable<ScheduleDefault>> Search(string searchCriteria);
}