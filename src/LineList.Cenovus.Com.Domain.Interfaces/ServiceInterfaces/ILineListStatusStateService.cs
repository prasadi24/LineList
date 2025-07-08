using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILineListStatusStateService
    {
        Task<IEnumerable<LineListStatusState>> GetAll();

        Task<LineListStatusState> GetById(Guid id);

        Task<LineListStatusState> Add(LineListStatusState lineListStatusState);

        Task<LineListStatusState> Update(LineListStatusState lineListStatusState);

        Task<bool> Remove(LineListStatusState lineListStatusState);

        //Task<IEnumerable<LineListStatusState>> Search(string searchCriteria);
    }
}