using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILineListModelRepository : IRepository<LineListModel>
    {
        new Task<List<LineListModel>> GetAll();

        new Task<LineListModel> GetById(Guid id);
        Task<List<LineListModel>> GetDocumentNumberAsync(Guid facilityId,Guid projectTypeId,Guid epCompanyId,Guid epProjectId,Guid cenovusProjectId);

        int GetCount();
    }
}