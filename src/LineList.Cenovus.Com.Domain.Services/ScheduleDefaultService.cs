using LineList.Cenovus.Com.Domain.Models;

public class ScheduleDefaultService : IScheduleDefaultService
{
    private readonly IScheduleDefaultRepository _repository;

    public ScheduleDefaultService(IScheduleDefaultRepository repository) => _repository = repository;

    public async Task<IEnumerable<ScheduleDefault>> GetAll() => await _repository.GetAll();

    public async Task<ScheduleDefault> GetById(Guid id) => await _repository.GetById(id);

    public async Task<ScheduleDefault> Add(ScheduleDefault entity)
    {
        await _repository.Add(entity);
        return entity;
    }

    public async Task<ScheduleDefault> Update(ScheduleDefault entity)
    {
        await _repository.Update(entity);
        return entity;
    }

    public async Task<bool> Remove(ScheduleDefault entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<ScheduleDefault>> Search(string searchCriteria) =>
        await _repository.Search(c => c.Notes.Contains(searchCriteria));

    public void Dispose() => _repository?.Dispose();
}