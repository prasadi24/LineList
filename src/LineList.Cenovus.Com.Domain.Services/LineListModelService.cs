using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineListModelList.Cenovus.Com.Domain.Services
{
	public class LineListModelService : ILineListModelService
	{
		private readonly ILineListModelRepository _lineListModelRepository;

		public LineListModelService(ILineListModelRepository lineListModelRepository)
		{
			_lineListModelRepository = lineListModelRepository;
		}

		public async Task<IEnumerable<LineListModel>> GetAll()
		{
			return await _lineListModelRepository.GetAll();
		}

		public async Task<LineListModel> GetById(Guid id)
		{
			return await _lineListModelRepository.GetById(id);
		}

		public async Task<LineListModel> Add(LineListModel lineListModel)
		{
			await _lineListModelRepository.Add(lineListModel);
			return lineListModel;
		}

		public async Task<LineListModel> Update(LineListModel lineListModel)
		{
			await _lineListModelRepository.Update(lineListModel);
			return lineListModel;
		}

		public async Task<bool> Remove(LineListModel lineListModel)
		{
			await _lineListModelRepository.Remove(lineListModel);
			return true;
		}

		public async Task<IEnumerable<LineListModel>> Search(string searchCriteria)
		{
			return await _lineListModelRepository.Search(c => c.DocumentNumber.Contains(searchCriteria));
		}

		public void Dispose()
		{
			_lineListModelRepository?.Dispose();
		}

		public int GetCount()
		{
			return _lineListModelRepository.GetCount();
		}

		public async Task<List<LineListModel>> GetDocumentNumberAsync(Guid facilityId, Guid projectTypeId, Guid epCompanyId, Guid epProjectId, Guid cenovusProjectId)
		{
			return await _lineListModelRepository.GetDocumentNumberAsync(facilityId, projectTypeId, epCompanyId, epProjectId, cenovusProjectId);
		}
    }
}