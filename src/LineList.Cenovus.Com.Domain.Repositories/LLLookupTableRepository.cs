using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LLLookupTableRepository : Repository<LLLookupTable>, ILLLookupTableRepository
    {
        private readonly LineListDbContext _context;

        public LLLookupTableRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        // Custom repository methods specific to LLLookupTable can be added here if necessary
    }
}