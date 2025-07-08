using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
	public interface ILineListModelService
	{
		Task<IEnumerable<LineListModel>> GetAll();

		Task<LineListModel> GetById(Guid id);

		Task<LineListModel> Add(LineListModel entity);

		Task<LineListModel> Update(LineListModel entity);

		Task<bool> Remove(LineListModel entity);

		Task<IEnumerable<LineListModel>> Search(string searchCriteria);

        Task<List<LineListModel>> GetDocumentNumberAsync(Guid facilityId, Guid projectTypeId, Guid epCompanyId, Guid epProjectId, Guid cenovusProjectId);

        int GetCount();
	}
}