using LineList.Cenovus.Com.API.DataTransferObjects.Line;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILineRevisionService
    {
        Task<IEnumerable<LineRevision>> GetAll();
        Task<bool> HasRevisionsForProject(Guid epProjectId);
        Task<List<LineRevision>> GetByIds(List<Guid> ids);
        Task<LineRevision> GetById(Guid id);
        Task<List<LineRevision>> GetByLineId(Guid lineId);

        Task<List<LineRevision>> GetByEpProjectId(Guid epProjectId);
        Task<List<LineRevision>> GetByLineListRevisionId(Guid lineListRevisionId);
        Task<bool> ExistLineRevisions(Guid revisionId, Guid locationId, Guid commodityId, string sequenceNumber, int? childNumber);

        Task<LineRevision> Add(LineRevision lineRevision);
        Task<LineRevision> AddWithoutSave(LineRevision lineRevision);

        Task<LineRevision> Update(LineRevision lineRevision);

        Task<LineRevision> UpdateStatus(List<KeyValuePair<Guid, bool>> activeSettings);

        Task<int> SaveChanges();

        Task<bool> Remove(LineRevision lineRevision);

        Task<IEnumerable<LineRevision>> Search(string searchCriteria);
        Task<List<LineResultDto>> GetFilteredLineRevisions(
            Guid? selectedFacilityId,
            Guid? SelectedSpecificationId,
            Guid? selectedLocationId,
            Guid? selectedCommodityId,
            Guid? selectedAreaId,
            Guid? selectedCenovusProjectId,
            Guid? selectedEPProjectId,
            Guid? selectedPipeSpecificationId,
            Guid? SelectedLineStatusId,
            bool showDrafts,
            bool showOnlyActive,
            string selectedDocumentNumberId,
            string selectedModularID,
            string SelectedSequenceNumber,
            Guid? selectedProjectTypeId);
        Task<List<LineRevision>> GetCheckOutLines(Guid[] ids, string userName);
        Task<List<LineRevision>> GetCheckInLines(Guid[] ids, string userName, bool admin);
        Task<Guid[]> GetDiscardableIds(Guid[] ids, string userName);

        /// <summary>
        /// Returns true if any LineRevision in that revision is currently checked out.
        /// </summary>
        Task<bool> AnyCheckedOutAsync(Guid lineListRevisionId);
    }
}