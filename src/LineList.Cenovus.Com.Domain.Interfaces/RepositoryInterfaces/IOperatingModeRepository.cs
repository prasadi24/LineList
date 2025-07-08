using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IOperatingModeRepository : IRepository<OperatingMode>
    {
        new Task<List<OperatingMode>> GetAll();

        new Task<OperatingMode> GetById(Guid id);
        Task<OperatingMode?> GetOperatingModeForPrimary();
        bool HasDependencies(Guid id);
    }
}