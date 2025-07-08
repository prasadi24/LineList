using LineList.Cenovus.Com.API.DataTransferObjects.EpProject;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpProjectRepository : IRepository<EpProject>
    {
        new Task<List<EpProject>> GetAll();

        new Task<EpProject> GetById(Guid id);
        new Task<List<EpProjectResultDto>> GetDataSource(bool showActive);
    }
}