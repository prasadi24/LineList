using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ValidationRuleRepository : Repository<ValidationRule>, IValidationRuleRepository
    {
        private readonly LineListDbContext _context;

        public ValidationRuleRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        // Additional custom repository methods specific to ValidationRule can be added here if necessary
    }
}