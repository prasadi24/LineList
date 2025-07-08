using LineList.Cenovus.Com.API.DataTransferObjects.LineList;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
	public class LineListRevisionService : ILineListRevisionService
	{
		private readonly ILineListRevisionRepository _lineListRevisionRepository;

		public LineListRevisionService(ILineListRevisionRepository lineListRevisionRepository)
		{
			_lineListRevisionRepository = lineListRevisionRepository;
		}

		public async Task<IEnumerable<LineListRevision>> GetAll()
		{
			return await _lineListRevisionRepository.GetAll();
		}
        public async Task<IEnumerable<LineListRevision>> GetAllLineListRevisions()
        {
            return await _lineListRevisionRepository.GetAllLineListRevisions();
        }
        public async Task<List<LineListRevision>> GetReservedByProjectId(Guid epProjectId)
        {
            return await _lineListRevisionRepository.GetReservedByProjectId(epProjectId);
        }

        public async Task<LineListRevision> GetReservedByLineListId(Guid lineListId)
        {
            return await _lineListRevisionRepository.GetReservedByLineListId(lineListId);
        }

        public async Task<LineListRevision> GetById(Guid id)
		{
			return await _lineListRevisionRepository.GetById(id);
		}

		public async Task<LineListRevision> Add(LineListRevision lineListRevision)
		{
			await _lineListRevisionRepository.Add(lineListRevision);
			return lineListRevision;
		}

		public async Task<LineListRevision> Update(LineListRevision lineListRevision)
		{
			await _lineListRevisionRepository.Update(lineListRevision);
			return lineListRevision;
		}

		public async Task<bool> Remove(LineListRevision lineListRevision)
		{
			await _lineListRevisionRepository.Remove(lineListRevision);
			return true;
		}

		public async Task<IEnumerable<LineListRevision>> Search(string searchCriteria)
		{
			return await _lineListRevisionRepository.Search(c => c.Description.Contains(searchCriteria));
		}

		public void Dispose()
		{
			_lineListRevisionRepository?.Dispose();
		}

		public Task<List<LineListResultDto>> GetFilteredLineListRevisions(
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
	Guid? selectedProjectTypeId)
		{
			return _lineListRevisionRepository.GetFilteredLineListRevisionsNew(
				selectedFacilityId,
				lineListId,
				selectedLocationId,
				selectedAreaId,
				selectedEPId,
				selectedProjectId,
				selectedEPProjectId,
				selectedLineListStatusId,
				showDrafts,
				showOnlyActive,
				selectedDocumentNumberId,
				selectedModularID,
				selectedProjectTypeId
			);
		}

        public async Task<Guid> GetReservedLineListRevisionIdByProjectId(Guid epProjectId)
        {
            return await _lineListRevisionRepository.GetReservedLineListRevisionIdByProjectId(epProjectId);
        }
    }
}