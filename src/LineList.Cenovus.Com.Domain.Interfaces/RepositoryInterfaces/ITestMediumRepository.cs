using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ITestMediumRepository : IRepository<TestMedium>
    {
        new Task<List<TestMedium>> GetAll();

        new Task<TestMedium> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}