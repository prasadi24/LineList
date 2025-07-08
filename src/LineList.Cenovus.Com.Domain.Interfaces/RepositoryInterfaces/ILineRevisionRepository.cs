using LineList.Cenovus.Com.API.DataTransferObjects.Line;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILineRevisionRepository : IRepository<LineRevision>
    {
        new Task<List<LineRevision>> GetAll();

        Task<bool> HasRevisionsForProject(Guid epProjectId);
        Task<List<LineRevision>> GetByIds(List<Guid> ids);

        new Task<LineRevision> GetById(Guid id);
        Task<List<LineRevision>> GetByEpProjectId(Guid epProjectId);
        
        new Task<List<LineRevision>> GetByLineId(Guid lineId);
        new Task<List<LineRevision>> GetByLineListRevisionId(Guid lineListRevisionId);
        Task<bool> ExistLineRevisions(Guid revisionId, Guid locationId, Guid commodityId, string sequenceNumber, int? childNumber);
        Task<List<LineResultDto>> GetFilteredLineRevisions(
            Guid? facilityId, Guid? specificationId, Guid? locationId, Guid? commodityId,Guid? areaId,
            Guid? cenovusProjectId, Guid? epProjectId, Guid? pipeSpecificationId, Guid? lineStatusId,
            bool showDrafts, bool showOnlyActive, string documentNumber,
            string modularId,string sequenceNumber, Guid? projectTypeId);
        Task<List<LineRevision>> GetCheckOutLines(Guid[] ids, string userName);
        Task<List<LineRevision>> GetCheckInLines(Guid[] ids, string username, bool isAdmin);
        Task<Guid[]> GetDiscardableIds(Guid[] possibles, string userName);

        Task<LineRevision> UpdateStatus(List<KeyValuePair<Guid, bool>> activeSettings);
        Task<bool> AnyCheckedOutAsync(Guid lineListRevisionId);
    }
}