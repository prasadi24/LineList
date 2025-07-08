using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILineListStatusService
    {
        Task<IEnumerable<LineListStatus>> GetAll();

        Task<LineListStatus> GetById(Guid id);
        Task<Guid?> GetCancelledDraftId();

        Task<LineListStatus> Add(LineListStatus lineListStatus);

        Task<LineListStatus> Update(LineListStatus lineListStatus);

        Task<bool> Remove(LineListStatus lineListStatus);

        Task<IEnumerable<LineListStatus>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}