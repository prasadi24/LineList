using LineList.Cenovus.Com.Domain.Models;

public class PressureProtectionService : IPressureProtectionService
{
    private readonly IPressureProtectionRepository _repository;

    public PressureProtectionService(IPressureProtectionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PressureProtection>> GetAll() =>
        await _repository.GetAll();

    public async Task<PressureProtection> GetById(Guid id) =>
        await _repository.GetById(id);

    public async Task<PressureProtection> Add(PressureProtection entity)
    {
        await _repository.Add(entity);
        return entity;
    }

    public async Task<PressureProtection> Update(PressureProtection entity)
    {
        await _repository.Update(entity);
        return entity;
    }

    public async Task<bool> Remove(PressureProtection entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<PressureProtection>> Search(string searchCriteria) =>
        await _repository.Search(c => c.Name.Contains(searchCriteria));

    public void Dispose() => _repository?.Dispose();

    public bool HasDependencies(Guid id)
    {
        return _repository.HasDependencies(id);
    }
}