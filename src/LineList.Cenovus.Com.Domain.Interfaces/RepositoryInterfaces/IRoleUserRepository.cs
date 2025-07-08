using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IRoleUserRepository : IRepository<RoleUser>
{
    new Task<List<RoleUser>> GetAll();

    new Task<RoleUser> GetById(Guid id);
}