using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ValidationFieldRepository : Repository<ValidationField>, IValidationFieldRepository
    {
        private readonly LineListDbContext _context;

        public ValidationFieldRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        // Custom repository methods specific to ValidationField can be added here, if necessary
    }
}