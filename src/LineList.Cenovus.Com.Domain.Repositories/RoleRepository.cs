using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class RoleRepository : Repository<Role>, IRoleRepository
{
    public RoleRepository(LineListDbContext context) : base(context)
    {
    }
}