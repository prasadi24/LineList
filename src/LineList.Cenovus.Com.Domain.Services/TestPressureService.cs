using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class TestPressureService : ITestPressureService
    {
        private readonly ITestPressureRepository _testPressureRepository;

        public TestPressureService(ITestPressureRepository testPressureRepository)
        {
            _testPressureRepository = testPressureRepository;
        }

        public async Task<IEnumerable<TestPressure>> GetAll()
        {
            return await _testPressureRepository.GetAll();
        }

        public async Task<TestPressure> GetById(Guid id)
        {
            return await _testPressureRepository.GetById(id);
        }

        public async Task<TestPressure> Add(TestPressure testPressure)
        {
            // Prevent adding a duplicate TestPressure entry based on Name
            if (_testPressureRepository.Search(c => c.Name == testPressure.Name).Result.Any())
                return null;

            await _testPressureRepository.Add(testPressure);
            return testPressure;
        }

        public async Task<TestPressure> Update(TestPressure testPressure)
        {
            // Prevent updating to a duplicate TestPressure entry based on Name
            if (_testPressureRepository.Search(c => c.Name == testPressure.Name && c.Id != testPressure.Id).Result.Any())
                return null;

            await _testPressureRepository.Update(testPressure);
            return testPressure;
        }

        public async Task<bool> Remove(TestPressure testPressure)
        {
            await _testPressureRepository.Remove(testPressure);
            return true;
        }

        public async Task<IEnumerable<TestPressure>> Search(string searchCriteria)
        {
            return await _testPressureRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _testPressureRepository?.Dispose();
        }
    }
}