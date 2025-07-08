using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILineRevisionOperatingModeService
    {
        Task<IEnumerable<LineRevisionOperatingMode>> GetAll();

        Task<LineRevisionOperatingMode> GetById(Guid id);
        Task<List<LineRevisionOperatingMode>> GetOperatingModesByLineRevisionId(Guid lineRevisionId);
        Task<LineRevisionOperatingMode> GetPrimaryOperatingMode(Guid lineRevisionId);

        Task<LineRevisionOperatingMode> Add(LineRevisionOperatingMode lineRevisionOperatingMode);
        Task<LineRevisionOperatingMode> AddWithoutSave(LineRevisionOperatingMode lineRevisionOperatingMode);

        Task<LineRevisionOperatingMode> Update(LineRevisionOperatingMode lineRevisionOperatingMode);

        Task<bool> Remove(LineRevisionOperatingMode lineRevisionOperatingMode);

        //Task<IEnumerable<LineRevisionOperatingMode>> Search(string searchCriteria);
    }
}