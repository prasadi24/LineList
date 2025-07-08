using LineList.Cenovus.Com.Domain.Models;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetAll();

    Task<Role> GetById(Guid id);

    Task<Role> Add(Role role);

    Task<Role> Update(Role role);

    Task<bool> Remove(Role role);

    Task<IEnumerable<Role>> Search(string searchCriteria);
}