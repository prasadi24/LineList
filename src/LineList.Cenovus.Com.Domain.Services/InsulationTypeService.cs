using LineList.Cenovus.Com.Domain.Models;

public class InsulationTypeService : IInsulationTypeService
{
    private readonly IInsulationTypeRepository _insulationTypeRepository;

    public InsulationTypeService(IInsulationTypeRepository insulationTypeRepository)
    {
        _insulationTypeRepository = insulationTypeRepository;
    }

    public async Task<IEnumerable<InsulationType>> GetAll()
    {
        return await _insulationTypeRepository.GetAll();
    }

    public async Task<InsulationType> GetById(Guid id)
    {
        return await _insulationTypeRepository.GetById(id);
    }

    public async Task<InsulationType> Add(InsulationType insulationType)
    {
        if (_insulationTypeRepository.Search(c => c.Name == insulationType.Name).Result.Any())
            return null;

        await _insulationTypeRepository.Add(insulationType);
        return insulationType;
    }

    public async Task<InsulationType> Update(InsulationType insulationType)
    {
        if (_insulationTypeRepository.Search(c => c.Name == insulationType.Name && c.Id != insulationType.Id).Result.Any())
            return null;

        await _insulationTypeRepository.Update(insulationType);
        return insulationType;
    }

    public async Task<bool> Remove(InsulationType insulationType)
    {
        await _insulationTypeRepository.Remove(insulationType);
        return true;
    }

    public async Task<IEnumerable<InsulationType>> Search(string searchCriteria)
    {
        return await _insulationTypeRepository.Search(c => c.Name.Contains(searchCriteria));
    }

    public void Dispose()
    {
        _insulationTypeRepository?.Dispose();
    }

    public bool HasDependencies(Guid id)
    {
        return _insulationTypeRepository.HasDependencies(id);
    }
}