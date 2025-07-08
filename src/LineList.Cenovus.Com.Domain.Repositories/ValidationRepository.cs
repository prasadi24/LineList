using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ValidationRepository : Repository<Validation>, IValidationRepository
    {
        private readonly LineListDbContext _context;

        public ValidationRepository(LineListDbContext context) : base(context)
        {
        }
    }
}