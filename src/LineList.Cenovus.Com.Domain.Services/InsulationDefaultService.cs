using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class InsulationDefaultService : IInsulationDefaultService
    {
        private readonly IInsulationDefaultRepository _insulationDefaultRepository;

        public InsulationDefaultService(IInsulationDefaultRepository insulationDefaultRepository)
        {
            _insulationDefaultRepository = insulationDefaultRepository;
        }

        public async Task<IEnumerable<InsulationDefault>> GetAll()
        {
            return await _insulationDefaultRepository.GetAll();
        }

        public async Task<InsulationDefault> GetById(Guid id)
        {
            return await _insulationDefaultRepository.GetById(id);
        }

        public async Task<InsulationDefault> Add(InsulationDefault insulationDefault)
        {
            if (_insulationDefaultRepository.Search(c => c.Name == insulationDefault.Name).Result.Any())
                return null;

            await _insulationDefaultRepository.Add(insulationDefault);
            return insulationDefault;
        }

        public async Task<InsulationDefault> Update(InsulationDefault insulationDefault)
        {
            if (_insulationDefaultRepository.Search(c => c.Name == insulationDefault.Name && c.Id != insulationDefault.Id).Result.Any())
                return null;

            await _insulationDefaultRepository.Update(insulationDefault);
            return insulationDefault;
        }

        public async Task<bool> Remove(InsulationDefault insulationDefault)
        {
            await _insulationDefaultRepository.Remove(insulationDefault);
            return true;
        }

        public async Task<IEnumerable<InsulationDefault>> Search(string searchCriteria)
        {
            return await _insulationDefaultRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _insulationDefaultRepository?.Dispose();
        }
    }
}