using LineList.Cenovus.Com.API.DataTransferObjects.Line;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LineRevisionService : ILineRevisionService
    {
        private readonly ILineRevisionRepository _lineRevisionRepository;

        public LineRevisionService(ILineRevisionRepository lineRevisionRepository)
        {
            _lineRevisionRepository = lineRevisionRepository;
        }

        public async Task<IEnumerable<LineRevision>> GetAll()
        {
            return await _lineRevisionRepository.GetAll();
        }
        public async Task<bool> HasRevisionsForProject(Guid epProjectId)
        {
            return await _lineRevisionRepository.HasRevisionsForProject(epProjectId);
        }
        public async Task<List<LineRevision>> GetByIds(List<Guid> ids)
        {
            return await _lineRevisionRepository.GetByIds(ids);
        }
        public async Task<List<LineRevision>> GetCheckOutLines(Guid[] ids, string userName)
        {
            return await _lineRevisionRepository.GetCheckOutLines(ids, userName);
        }
        public async Task<List<LineRevision>> GetCheckInLines(Guid[] ids, string userName, bool admin)
        {
            return await _lineRevisionRepository.GetCheckInLines(ids, userName, admin);
        }

        public async Task<Guid[]> GetDiscardableIds(Guid[] ids, string userName)
        {
            return await _lineRevisionRepository.GetDiscardableIds(ids, userName);
        }
        public async Task<LineRevision> GetById(Guid id)
        {
            return await _lineRevisionRepository.GetById(id);
        }
        public async Task<List<LineRevision>> GetByLineId(Guid lineId)
        {
            return await _lineRevisionRepository.GetByLineId(lineId);
        }

        public async Task<List<LineRevision>> GetByEpProjectId(Guid epProjectId)
        {
            return await _lineRevisionRepository.GetByEpProjectId(epProjectId);
        }

        public async Task<List<LineRevision>> GetByLineListRevisionId(Guid lineListRevisionId)
        {
            return await _lineRevisionRepository.GetByLineListRevisionId(lineListRevisionId);
        }
        public async Task<bool> ExistLineRevisions(Guid revisionId, Guid locationId, Guid commodityId, string sequenceNumber, int? childNumber)
        {
            return await _lineRevisionRepository.ExistLineRevisions(revisionId, locationId, commodityId, sequenceNumber, childNumber);
        }
        public async Task<LineRevision> Add(LineRevision lineRevision)
        {
            // Check if the LineRevision already exists, for example by checking its number or other unique property.
            //if (_lineRevisionRepository.Search(c => c.ModifiedBy == lineRevision.ModifiedBy).Result.Any())
            //    return null;

            await _lineRevisionRepository.Add(lineRevision);
            return lineRevision;
        }
        public async Task<LineRevision> AddWithoutSave(LineRevision lineRevision)
        {
            // Check if the LineRevision already exists, for example by checking its number or other unique property.
            //if (_lineRevisionRepository.Search(c => c.ModifiedBy == lineRevision.ModifiedBy).Result.Any())
            //    return null;

            await _lineRevisionRepository.AddWithoutSave(lineRevision);
            return lineRevision;
        }

        public async Task<LineRevision> Update(LineRevision lineRevision)
        {
            // Check for duplicates or other custom logic, if necessary
            if (_lineRevisionRepository.Search(c => c.ModifiedBy == lineRevision.ModifiedBy && c.ModifiedBy != lineRevision.ModifiedBy).Result.Any())
                return null;

            await _lineRevisionRepository.Update(lineRevision);
            return lineRevision;
        }

        public async Task<LineRevision> UpdateStatus(List<KeyValuePair<Guid, bool>> activeSettings)
        {            
           return await _lineRevisionRepository.UpdateStatus(activeSettings);
        }
        public async Task<int> SaveChanges()
        {
            return await _lineRevisionRepository.SaveChanges();
        }

        public async Task<bool> Remove(LineRevision lineRevision)
        {
            await _lineRevisionRepository.Remove(lineRevision);
            return true;
        }

        public async Task<IEnumerable<LineRevision>> Search(string searchCriteria)
        {
            return await _lineRevisionRepository.Search(c => c.ModifiedBy.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _lineRevisionRepository?.Dispose();
        }

        public Task<List<LineResultDto>> GetFilteredLineRevisions(
     Guid? selectedFacilityId,
            Guid? SelectedSpecificationId,
            Guid? selectedLocationId,
            Guid? selectedCommodityId,
            Guid? selectedAreaId,
            Guid? selectedcenovusProjectId,
            Guid? selectedEPProjectId,
            Guid? selectedPipeSpecificationId,
            Guid? SelectedLineStatusId,
            bool showDrafts,
            bool showOnlyActive,
            string selectedDocumentNumberId,
            string selectedModularID,
            string SelectedSequenceNumber,
            Guid? selectedProjectTypeId)
        {
            return _lineRevisionRepository.GetFilteredLineRevisions(
                selectedFacilityId,
                SelectedSpecificationId,
                selectedLocationId,
                selectedCommodityId,
                selectedAreaId,
                selectedcenovusProjectId,
                selectedEPProjectId,
                selectedPipeSpecificationId,
                SelectedLineStatusId,
                showDrafts,
                showOnlyActive,
                selectedDocumentNumberId,
                selectedModularID,
                SelectedSequenceNumber,
                selectedProjectTypeId
            );
        }
        public async Task<bool> AnyCheckedOutAsync(Guid lineListRevisionId)
        {
            return await _lineRevisionRepository.AnyCheckedOutAsync(lineListRevisionId);
        }

    }
}