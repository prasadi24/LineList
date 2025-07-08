using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IStressAnalysisService
    {
        Task<IEnumerable<StressAnalysis>> GetAll();

        Task<StressAnalysis> GetById(Guid id);

        Task<StressAnalysis> Add(StressAnalysis stressAnalysis);

        Task<StressAnalysis> Update(StressAnalysis stressAnalysis);

        Task<bool> Remove(StressAnalysis stressAnalysis);

        Task<IEnumerable<StressAnalysis>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}