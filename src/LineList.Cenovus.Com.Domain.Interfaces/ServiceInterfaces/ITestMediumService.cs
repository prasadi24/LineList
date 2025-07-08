using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ITestMediumService
    {
        Task<IEnumerable<TestMedium>> GetAll();

        Task<TestMedium> GetById(Guid id);

        Task<TestMedium> Add(TestMedium testMedium);

        Task<TestMedium> Update(TestMedium testMedium);

        Task<bool> Remove(TestMedium testMedium);

        Task<IEnumerable<TestMedium>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}