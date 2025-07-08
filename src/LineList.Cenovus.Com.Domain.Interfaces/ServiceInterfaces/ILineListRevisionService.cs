using LineList.Cenovus.Com.API.DataTransferObjects.LineList;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
	public interface ILineListRevisionService
	{
		Task<IEnumerable<LineListRevision>> GetAll();
        Task<IEnumerable<LineListRevision>> GetAllLineListRevisions();
        Task<List<LineListRevision>> GetReservedByProjectId(Guid epProjectId);

		Task<LineListRevision> GetReservedByLineListId(Guid lineListId);
        Task<LineListRevision> GetById(Guid id);

		Task<LineListRevision> Add(LineListRevision lineListRevision);

		Task<LineListRevision> Update(LineListRevision lineListRevision);

		Task<bool> Remove(LineListRevision lineListRevision);

		Task<IEnumerable<LineListRevision>> Search(string searchCriteria);

		Task<List<LineListResultDto>> GetFilteredLineListRevisions(
									Guid? selectedFacilityId,
									Guid? lineListId,
									Guid? selectedLocationId,
									Guid? selectedAreaId,
									Guid? selectedEPId,
									Guid? selectedProjectId,
									Guid? selectedEPProjectId,
									Guid? selectedLineListStatusId,
									bool showDrafts,
									bool showOnlyActive,
									string selectedDocumentNumberId,
									string selectedModularID,
									Guid? selectedProjectTypeId);
		Task<Guid> GetReservedLineListRevisionIdByProjectId(Guid epProjectId);

    }
}