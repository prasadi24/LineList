using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class TestPressureRepository : Repository<TestPressure>, ITestPressureRepository
    {
        private readonly LineListDbContext _context;

        public TestPressureRepository(LineListDbContext context) : base(context)
        {
        }
    }
}