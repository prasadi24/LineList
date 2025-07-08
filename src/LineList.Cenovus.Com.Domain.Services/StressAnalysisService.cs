using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class StressAnalysisService : IStressAnalysisService
    {
        private readonly IStressAnalysisRepository _stressAnalysisRepository;

        public StressAnalysisService(IStressAnalysisRepository stressAnalysisRepository)
        {
            _stressAnalysisRepository = stressAnalysisRepository;
        }

        public async Task<IEnumerable<StressAnalysis>> GetAll()
        {
            return await _stressAnalysisRepository.GetAll();
        }

        public async Task<StressAnalysis> GetById(Guid id)
        {
            return await _stressAnalysisRepository.GetById(id);
        }

        public async Task<StressAnalysis> Add(StressAnalysis stressAnalysis)
        {
            // Prevent adding a duplicate StressAnalysis entry based on Name
            if (_stressAnalysisRepository.Search(c => c.Name == stressAnalysis.Name).Result.Any())
                return null;

            await _stressAnalysisRepository.Add(stressAnalysis);
            return stressAnalysis;
        }

        public async Task<StressAnalysis> Update(StressAnalysis stressAnalysis)
        {
            // Prevent updating to a duplicate StressAnalysis entry based on Name
            if (_stressAnalysisRepository.Search(c => c.Name == stressAnalysis.Name && c.Id != stressAnalysis.Id).Result.Any())
                return null;

            await _stressAnalysisRepository.Update(stressAnalysis);
            return stressAnalysis;
        }

        public async Task<bool> Remove(StressAnalysis stressAnalysis)
        {
            await _stressAnalysisRepository.Remove(stressAnalysis);
            return true;
        }

        public async Task<IEnumerable<StressAnalysis>> Search(string searchCriteria)
        {
            return await _stressAnalysisRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _stressAnalysisRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _stressAnalysisRepository.HasDependencies(id);
        }
    }
}