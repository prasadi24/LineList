using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IScheduleRepository : IRepository<Schedule>
{
    new Task<List<Schedule>> GetAll();

    new Task<Schedule> GetById(Guid id);

    bool HasDependencies(Guid id);
}