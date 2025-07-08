using LineList.Cenovus.Com.API.DataTransferObjects.LineList;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
	public interface ILineListRevisionRepository : IRepository<LineListRevision>
	{
		new Task<List<LineListRevision>> GetAll();
		Task<List<LineListRevision>> GetAllLineListRevisions();
        Task<List<LineListRevision>> GetReservedByProjectId(Guid epProjectId);

		Task<LineListRevision> GetReservedByLineListId(Guid lineListId);

        new Task<LineListRevision> GetById(Guid id);

		Task<List<LineListRevision>> GetFilteredLineListRevisions(
	   Guid? facilityId, Guid? lineListId, Guid? locationId, Guid? areaId,
	   Guid? epId, Guid? projectId, Guid? epProjectId, Guid? statusId,
	   bool showDrafts, bool showOnlyActive, string documentNumber,
	   string modularId, Guid? projectTypeId);

        Task<List<LineListResultDto>> GetFilteredLineListRevisionsNew(
		Guid? facilityId, Guid? lineListId, Guid? locationId, Guid? areaId,
		Guid? epCompanyId, Guid? projectId, Guid? epProjectId, Guid? statusId,
		bool showDrafts, bool showOnlyActive, string documentNumber,
		string modularId, Guid? projectTypeId);

        Task<Guid> GetReservedLineListRevisionIdByProjectId(Guid epProjectId);

    }
}