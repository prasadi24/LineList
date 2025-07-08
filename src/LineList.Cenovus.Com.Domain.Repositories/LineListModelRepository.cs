using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class LineListModelRepository : Repository<LineListModel>, ILineListModelRepository
{
	private readonly LineListDbContext _context;

	public LineListModelRepository(LineListDbContext context) : base(context)
	{
		_context = context;
	}

	public int GetCount()
	{
		return _context.LineLists.Count();
	}

    public async Task<List<LineListModel>> GetDocumentNumberAsync(
    Guid facilityId,
    Guid projectTypeId,
    Guid epCompanyId,
    Guid epProjectId,
    Guid cenovusProjectId)
    {
        // Filter revisions at the database level
        var filteredRevs = _context.LineListRevisions.AsQueryable(); // returns IQueryable<LineListRevision>

        if (facilityId != Guid.Empty)
            filteredRevs = filteredRevs.Where(m => m.EpProject.CenovusProject.FacilityId == facilityId);

        if (projectTypeId != Guid.Empty)
            filteredRevs = filteredRevs.Where(m => m.EpProject.CenovusProject.ProjectTypeId == projectTypeId);

        if (epCompanyId != Guid.Empty)
            filteredRevs = filteredRevs.Where(m => m.EpCompanyId == epCompanyId);

        if (epProjectId != Guid.Empty)
            filteredRevs = filteredRevs.Where(m => m.EpProjectId == epProjectId);

        if (cenovusProjectId != Guid.Empty)
            filteredRevs = filteredRevs.Where(m => m.EpProject.CenovusProjectId == cenovusProjectId);

        // Select just the LineListIds
        var lineListIds = filteredRevs.Select(r => r.LineListId).Distinct().ToList();

        // Now fetch only matching LineLists
        var result =  _context.LineLists.AsQueryable().Where(m => lineListIds.Contains(m.Id)).OrderBy(m => m.DocumentNumber).ToList();

        return result;
    }

}