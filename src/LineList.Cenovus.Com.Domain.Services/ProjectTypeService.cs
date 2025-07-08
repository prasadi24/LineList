using LineList.Cenovus.Com.Domain.Models;

public class ProjectTypeService : IProjectTypeService
{
    private readonly IProjectTypeRepository _repository;

    public ProjectTypeService(IProjectTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProjectType>> GetAll() =>
        await _repository.GetAll();

    public async Task<ProjectType> GetById(Guid id) =>
        await _repository.GetById(id);

    public async Task<ProjectType> Add(ProjectType entity)
    {
        await _repository.Add(entity);
        return entity;
    }

    public async Task<ProjectType> Update(ProjectType entity)
    {
        await _repository.Update(entity);
        return entity;
    }

    public async Task<bool> Remove(ProjectType entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<ProjectType>> Search(string searchCriteria) =>
        await _repository.Search(c => c.Name.Contains(searchCriteria));

    public void Dispose() => _repository?.Dispose();

    public bool HasDependencies(Guid id)
    {
      return   _repository.HasDependencies(id);
    }
}