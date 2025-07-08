using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
	public class LineService : ILineService
	{
		private readonly ILineRepository _lineRepository;

		public LineService(ILineRepository lineRepository)
		{
			_lineRepository = lineRepository;
		}

		public async Task<IEnumerable<Line>> GetAll()
		{
			return await _lineRepository.GetAll();
		}

		public async Task<Line> GetById(Guid id)
		{
			return await _lineRepository.GetById(id);
		}

		public async Task<Line> Add(Line line)
		{
			await _lineRepository.Add(line);
			return line;
		}

		public async Task<Line> Update(Line line)
		{
			await _lineRepository.Update(line);
			return line;
		}

		public async Task<bool> Remove(Line line)
		{
			await _lineRepository.Remove(line);
			return true;
		}

		public async Task<IEnumerable<Line>> Search(string searchCriteria)
		{
			return await _lineRepository.Search(c => c.Location.Name.Contains(searchCriteria));
		}

        public async Task<IEnumerable<Line>> GetByLocationAndCommodity(Guid locationId, Guid commodityId)
        {
            return await _lineRepository.GetByLocationAndCommodity(locationId, commodityId);
        }
        public async Task<int> GetNextChildNumber(Guid locationId, Guid commodityId, string sequenceNumber)
        {
            return await _lineRepository.GetNextChildNumber(locationId, commodityId, sequenceNumber);
        }
        public async Task<HashSet<(Guid, Guid, string)>> GetParentLineLookupAsync()
        {
            return await _lineRepository.GetParentLineLookupAsync();
        }
        public async Task ImportLinesFromExcel(Stream fileStream, Guid lineListRevisionId)
        {
            await _lineRepository.ImportLinesFromExcel(fileStream, lineListRevisionId);
        }

        public void Dispose()
		{
			_lineRepository?.Dispose();
		}

		public int GetCount()
		{
			return _lineRepository.Count();
		}
	}
}