using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITestPressureRepository : IRepository<TestPressure>
    {
        new Task<List<TestPressure>> GetAll();

        new Task<TestPressure> GetById(Guid id);
    }
}