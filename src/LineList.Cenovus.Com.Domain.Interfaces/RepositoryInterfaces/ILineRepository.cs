using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface ILineRepository : IRepository<Line>
{
    Task<IEnumerable<Line>> GetByLocationAndCommodity(Guid locationId, Guid commodityId);
    int Count();

	new Task<List<Line>> GetAll();

	new Task<Line> GetById(Guid id);
    Task<int> GetNextChildNumber(Guid locationId, Guid commodityId, string sequenceNumber);
    Task<HashSet<(Guid, Guid, string)>> GetParentLineLookupAsync();
    Task ImportLinesFromExcel(Stream fileStream, Guid lineListRevisionId);
}