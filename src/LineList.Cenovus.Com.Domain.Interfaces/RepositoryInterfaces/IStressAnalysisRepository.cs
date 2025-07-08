using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IStressAnalysisRepository : IRepository<StressAnalysis>
    {
        new Task<List<StressAnalysis>> GetAll();

        new Task<StressAnalysis> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}