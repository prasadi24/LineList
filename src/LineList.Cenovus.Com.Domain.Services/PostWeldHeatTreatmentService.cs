using LineList.Cenovus.Com.Domain.Models;

public class PostWeldHeatTreatmentService : IPostWeldHeatTreatmentService
{
    private readonly IPostWeldHeatTreatmentRepository _repository;

    public PostWeldHeatTreatmentService(IPostWeldHeatTreatmentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<PostWeldHeatTreatment>> GetAll() =>
        await _repository.GetAll();

    public async Task<PostWeldHeatTreatment> GetById(Guid id) =>
        await _repository.GetById(id);

    public async Task<PostWeldHeatTreatment> Add(PostWeldHeatTreatment entity)
    {
        await _repository.Add(entity);
        return entity;
    }

    public async Task<PostWeldHeatTreatment> Update(PostWeldHeatTreatment entity)
    {
        await _repository.Update(entity);
        return entity;
    }

    public async Task<bool> Remove(PostWeldHeatTreatment entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<PostWeldHeatTreatment>> Search(string searchCriteria) =>
        await _repository.Search(c => c.Name.Contains(searchCriteria));

    public void Dispose() => _repository?.Dispose();

    public bool HasDependencies(Guid id)
    {
       return _repository.HasDependencies(id);
    }
}