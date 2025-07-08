using LineList.Cenovus.Com.Domain.Models;

public interface IScheduleService
{
    Task<IEnumerable<Schedule>> GetAll();

    Task<Schedule> GetById(Guid id);

    Task<Schedule> Add(Schedule schedule);

    Task<Schedule> Update(Schedule schedule);

    Task<bool> Remove(Schedule schedule);

    Task<IEnumerable<Schedule>> Search(string searchCriteria);

    bool HasDependencies(Guid id);
}