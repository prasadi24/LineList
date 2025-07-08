using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LLLookupTableNoDescriptionRepository : Repository<LLLookupTableNoDescription>, ILLLookupTableNoDescriptionRepository
    {
        private readonly LineListDbContext _context;

        public LLLookupTableNoDescriptionRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        // Custom repository methods specific to LLLookupTableNoDescription can be added here if necessary
    }
}