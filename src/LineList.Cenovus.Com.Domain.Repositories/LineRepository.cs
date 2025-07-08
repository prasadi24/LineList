using ClosedXML.Excel;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

public class LineRepository : Repository<Line>, ILineRepository
{
	private readonly LineListDbContext _context;

	public LineRepository(LineListDbContext context) : base(context)
	{
		_context = context;
	}

	public int Count()
	{
		return _context.Lines.Count();
	}

    public async Task<IEnumerable<Line>> GetByLocationAndCommodity(Guid locationId, Guid commodityId)
    {
        return await _context.Lines
            .Include(l => l.Location)
            .Include(l => l.Commodity)
            .Where(l => l.LocationId == locationId && l.CommodityId == commodityId)
            .ToListAsync();
    }

    public async Task<int> GetNextChildNumber(Guid locationId, Guid commodityId, string sequenceNumber)
    {
        var childNumbers = await Db.Lines
        .Where(m => m.LocationId == locationId
                    && m.CommodityId == commodityId
                    && m.SequenceNumber == sequenceNumber)
        .Select(m => m.ChildNumber)
        .ToListAsync();

        return (childNumbers.Any() ? childNumbers.Max() : 0) + 1;
    }

    public async Task<HashSet<(Guid, Guid, string)>> GetParentLineLookupAsync()
    {
        return new HashSet<(Guid, Guid, string)>(
            await Db.Lines
                .Where(l => l.ChildNumber != 0)
                .Select(l => new ValueTuple<Guid, Guid, string>(l.LocationId, l.CommodityId, l.SequenceNumber))
                .ToListAsync()
        );
    }
    public async Task ImportLinesFromExcel(Stream fileStream, Guid lineListRevisionId)
    {
        var lines = new List<Line>();

        using (var workbook = new XLWorkbook(fileStream))
        {
            var worksheet = workbook.Worksheet(1); // First worksheet
            var rows = worksheet.RowsUsed();

            bool isHeader = true;

            foreach (var row in rows)
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue; // Skip header
                }

                var line = new Line
                {
                    Id = Guid.NewGuid(),
                    SequenceNumber = row.Cell(1).GetString(),
                    CommodityId = Guid.TryParse(row.Cell(4).GetString(), out var commId) ? commId : Guid.Empty,
                    LocationId = Guid.TryParse(row.Cell(5).GetString(), out var locId) ? locId : Guid.Empty
                };

                lines.Add(line);
            }
        }

        await Db.Lines.AddRangeAsync(lines);
        await Db.SaveChangesAsync();
    }
}
