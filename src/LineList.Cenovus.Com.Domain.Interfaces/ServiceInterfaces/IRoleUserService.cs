using LineList.Cenovus.Com.Domain.Models;

public interface IRoleUserService
{
    Task<IEnumerable<RoleUser>> GetAll();

    Task<RoleUser> GetById(Guid id);

    Task<RoleUser> Add(RoleUser roleUser);

    Task<RoleUser> Update(RoleUser roleUser);

    Task<bool> Remove(RoleUser roleUser);

    Task<IEnumerable<RoleUser>> Search(string searchCriteria);
}