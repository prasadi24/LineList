using LineList.Cenovus.Com.Domain.Models;

public class ReferenceLineService : IReferenceLineService
{
    private readonly IReferenceLineRepository _repository;

    public ReferenceLineService(IReferenceLineRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ReferenceLine>> GetAll() =>
        await _repository.GetAll();

    public async Task<ReferenceLine> GetById(Guid id) =>
        await _repository.GetById(id);

    public async Task<ReferenceLine> Add(ReferenceLine entity)
    {
        await _repository.Add(entity);
        return entity;
    }

    public async Task<ReferenceLine> Update(ReferenceLine entity)
    {
        await _repository.Update(entity);
        return entity;
    }

    public async Task<bool> Remove(ReferenceLine entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<ReferenceLine>> Search(string searchCriteria) =>
        await _repository.Search(c => c.LineListRevision.ReviewedBy.Contains(searchCriteria));

    public void Dispose() => _repository?.Dispose();
}