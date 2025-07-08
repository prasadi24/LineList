using LineList.Cenovus.Com.API.DataTransferObjects.EpProject;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpProjectService
    {
        Task<IEnumerable<EpProject>> GetAll();

        Task<EpProject> GetById(Guid id);
        Task<IEnumerable<EpProjectResultDto>> GetDataSource(bool showActive);
        Task<EpProject> Add(EpProject epProject);

        Task<EpProject> Update(EpProject epProject);

        Task<bool> Remove(EpProject epProject);

        Task<IEnumerable<EpProject>> Search(string searchCriteria);
    }
}