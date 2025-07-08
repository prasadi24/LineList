using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILineStatusService
    {
        Task<IEnumerable<LineStatus>> GetAll();

        Task<LineStatus> GetById(Guid id);
        Task<Guid> GetStatusIdByName(string name);
        Task<Guid> GetDeletedStatusId();

        Task<LineStatus> Add(LineStatus lineStatus);

        Task<LineStatus> Update(LineStatus lineStatus);

        Task<bool> Remove(LineStatus lineStatus);

        Task<IEnumerable<LineStatus>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}