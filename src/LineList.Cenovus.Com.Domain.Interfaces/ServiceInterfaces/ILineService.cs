using LineList.Cenovus.Com.Domain.Models;

public interface ILineService
{
	Task<IEnumerable<Line>> GetAll();

	Task<Line> GetById(Guid id);

	Task<Line> Add(Line line);

	Task<Line> Update(Line line);

	Task<bool> Remove(Line line);

	Task<IEnumerable<Line>> Search(string searchCriteria);

    Task<IEnumerable<Line>> GetByLocationAndCommodity(Guid locationId, Guid commodityId);
	Task<int> GetNextChildNumber(Guid locationId, Guid commodityId, string sequenceNumber);
	Task<HashSet<(Guid, Guid, string)>> GetParentLineLookupAsync();
	Task ImportLinesFromExcel(Stream fileStream, Guid lineListRevisionId);

    int GetCount();
}