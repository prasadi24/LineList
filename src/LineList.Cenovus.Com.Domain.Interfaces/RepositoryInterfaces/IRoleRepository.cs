using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IRoleRepository : IRepository<Role>
{
    new Task<List<Role>> GetAll();

    new Task<Role> GetById(Guid id);
}