using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class RoleUserRepository : Repository<RoleUser>, IRoleUserRepository
{
    public RoleUserRepository(LineListDbContext context) : base(context)
    {
    }
}