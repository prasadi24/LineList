using LineList.Cenovus.Com.Domain.Models;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _repository;

    public RoleService(IRoleRepository repository) => _repository = repository;

    public async Task<IEnumerable<Role>> GetAll() => await _repository.GetAll();

    public async Task<Role> GetById(Guid id) => await _repository.GetById(id);

    public async Task<Role> Add(Role entity)
    {
        await _repository.Add(entity);
        return entity;
    }

    public async Task<Role> Update(Role entity)
    {
        await _repository.Update(entity);
        return entity;
    }

    public async Task<bool> Remove(Role entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<Role>> Search(string searchCriteria) =>
        await _repository.Search(c => c.Description.Contains(searchCriteria));

    public void Dispose() => _repository?.Dispose();
}