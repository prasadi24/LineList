using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IOperatingModeService
    {
        Task<IEnumerable<OperatingMode>> GetAll();

        Task<OperatingMode> GetById(Guid id);
        Task<OperatingMode?> GetOperatingModeForPrimary();
        Task<OperatingMode> Add(OperatingMode operatingMode);

        Task<OperatingMode> Update(OperatingMode operatingMode);

        Task<bool> Remove(OperatingMode operatingMode);

        Task<IEnumerable<OperatingMode>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}