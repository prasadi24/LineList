using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ITestPressureService
    {
        Task<IEnumerable<TestPressure>> GetAll();

        Task<TestPressure> GetById(Guid id);

        Task<TestPressure> Add(TestPressure testPressure);

        Task<TestPressure> Update(TestPressure testPressure);

        Task<bool> Remove(TestPressure testPressure);

        Task<IEnumerable<TestPressure>> Search(string searchCriteria);
    }
}