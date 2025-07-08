using LineList.Cenovus.Com.Domain.Models;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repository;

    public ScheduleService(IScheduleRepository repository) => _repository = repository;

    public async Task<IEnumerable<Schedule>> GetAll() => await _repository.GetAll();

    public async Task<Schedule> GetById(Guid id) => await _repository.GetById(id);

    public async Task<Schedule> Add(Schedule schedule)
    {
        if (_repository.Search(c => c.Name == schedule.Name).Result.Any())
            return null;

        await _repository.Add(schedule);
        return schedule;
    }

    public async Task<Schedule> Update(Schedule schedule)
    {
        if (_repository.Search(c => c.Name == schedule.Name && c.Id != schedule.Id).Result.Any())
            return null;

        await _repository.Update(schedule);
        return schedule;
    }
    
    public async Task<bool> Remove(Schedule entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<Schedule>> Search(string searchCriteria) =>
        await _repository.Search(c => c.Name.Contains(searchCriteria));

    public void Dispose() => _repository?.Dispose();

    public bool HasDependencies(Guid id)
    {
        return _repository.HasDependencies(id);
    }
}