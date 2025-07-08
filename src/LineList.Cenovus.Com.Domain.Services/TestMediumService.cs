using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class TestMediumService : ITestMediumService
    {
        private readonly ITestMediumRepository _testMediumRepository;

        public TestMediumService(ITestMediumRepository testMediumRepository)
        {
            _testMediumRepository = testMediumRepository;
        }

        public async Task<IEnumerable<TestMedium>> GetAll()
        {
            return await _testMediumRepository.GetAll();
        }

        public async Task<TestMedium> GetById(Guid id)
        {
            return await _testMediumRepository.GetById(id);
        }

        public async Task<TestMedium> Add(TestMedium testMedium)
        {
            // Prevent adding a duplicate TestMedium entry based on Name
            if (_testMediumRepository.Search(c => c.Name == testMedium.Name).Result.Any())
                return null;

            await _testMediumRepository.Add(testMedium);
            return testMedium;
        }

        public async Task<TestMedium> Update(TestMedium testMedium)
        {
            // Prevent updating to a duplicate TestMedium entry based on Name
            if (_testMediumRepository.Search(c => c.Name == testMedium.Name && c.Id != testMedium.Id).Result.Any())
                return null;

            await _testMediumRepository.Update(testMedium);
            return testMedium;
        }

        public async Task<bool> Remove(TestMedium testMedium)
        {
            await _testMediumRepository.Remove(testMedium);
            return true;
        }

        public async Task<IEnumerable<TestMedium>> Search(string searchCriteria)
        {
            return await _testMediumRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _testMediumRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _testMediumRepository.HasDependencies(id);
        }
    }
}