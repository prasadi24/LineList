using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpProjectInsulationDefaultService : IEpProjectInsulationDefaultService
    {
        private readonly IEpProjectInsulationDefaultRepository _epProjectInsulationDefaultRepository;

        public EpProjectInsulationDefaultService(IEpProjectInsulationDefaultRepository epProjectInsulationDefaultRepository)
        {
            _epProjectInsulationDefaultRepository = epProjectInsulationDefaultRepository;
        }

        public async Task<IEnumerable<EpProjectInsulationDefault>> GetAll()
        {
            return await _epProjectInsulationDefaultRepository.GetAll();
        }

        public async Task<EpProjectInsulationDefault> GetById(Guid id)
        {
            return await _epProjectInsulationDefaultRepository.GetById(id);
        }

        public async Task<EpProjectInsulationDefault> Add(EpProjectInsulationDefault epProjectInsulationDefault)
        {
            // Example condition for checking before adding
            if (_epProjectInsulationDefaultRepository.Search(c => c.Name == epProjectInsulationDefault.Name).Result.Any())
                return null;

            await _epProjectInsulationDefaultRepository.Add(epProjectInsulationDefault);
            return epProjectInsulationDefault;
        }

        public async Task<EpProjectInsulationDefault> Update(EpProjectInsulationDefault epProjectInsulationDefault)
        {
            await _epProjectInsulationDefaultRepository.Update(epProjectInsulationDefault);
            return epProjectInsulationDefault;
        }

        public async Task<bool> Remove(EpProjectInsulationDefault epProjectInsulationDefault)
        {
            await _epProjectInsulationDefaultRepository.Remove(epProjectInsulationDefault);
            return true;
        }

        public async Task<IEnumerable<EpProjectInsulationDefault>> Search(string searchCriteria)
        {
            return await _epProjectInsulationDefaultRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _epProjectInsulationDefaultRepository?.Dispose();
        }
    }
}