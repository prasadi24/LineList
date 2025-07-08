using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class TracingDesignNumberOfTracersRepository : Repository<TracingDesignNumberOfTracers>, ITracingDesignNumberOfTracersRepository
    {
        private readonly LineListDbContext _context;

        public TracingDesignNumberOfTracersRepository(LineListDbContext context) : base(context)
        {
        }

        public bool HasDependencies(Guid id)
        {
            return Db.InsulationDefaultDetails.Any(m => m.TracingDesignNumberOfTracersId == id)
                || Db.LineRevisionSegments.Any(m => m.TracingDesignNumberOfTracersId == id);
               
        }
    }
}