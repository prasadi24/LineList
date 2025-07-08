using LineList.Cenovus.Com.Domain.Models;

public class SegmentTypeService : ISegmentTypeService
{
    private readonly ISegmentTypeRepository _repository;

    public SegmentTypeService(ISegmentTypeRepository repository) => _repository = repository;

    public async Task<IEnumerable<SegmentType>> GetAll() => await _repository.GetAll();

    public async Task<SegmentType> GetById(Guid id) => await _repository.GetById(id);

    public async Task<SegmentType> Add(SegmentType entity)
    {
        await _repository.Add(entity);
        return entity;
    }

    public async Task<SegmentType> Update(SegmentType entity)
    {
        await _repository.Update(entity);
        return entity;
    }

    public async Task<bool> Remove(SegmentType entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<SegmentType>> Search(string searchCriteria) =>
        await _repository.Search(c => c.Name.Contains(searchCriteria));

    public void Dispose() => _repository?.Dispose();

    public bool HasDependencies(Guid id)
    {
        return  _repository.HasDependencies(id);
    }
}