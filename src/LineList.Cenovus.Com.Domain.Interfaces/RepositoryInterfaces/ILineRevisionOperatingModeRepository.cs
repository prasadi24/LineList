using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILineRevisionOperatingModeRepository : IRepository<LineRevisionOperatingMode>
    {
        new Task<List<LineRevisionOperatingMode>> GetAll();

        new Task<LineRevisionOperatingMode> GetById(Guid id);
        Task<List<LineRevisionOperatingMode>> GetOperatingModesByLineRevisionId(Guid lineRevisionId);
        Task<LineRevisionOperatingMode> GetPrimaryOperatingMode(Guid lineRevisionId);
    }
}