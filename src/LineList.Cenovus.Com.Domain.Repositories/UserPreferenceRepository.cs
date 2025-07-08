using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class UserPreferenceRepository : Repository<UserPreference>, IUserPreferenceRepository
    {
        private readonly LineListDbContext _context;

        public UserPreferenceRepository(LineListDbContext context) : base(context)
        {
        }
    }
}