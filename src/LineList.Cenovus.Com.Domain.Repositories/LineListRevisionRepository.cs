using DocumentFormat.OpenXml.Drawing;
using LineList.Cenovus.Com.API.DataTransferObjects.LineList;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
	public class LineListRevisionRepository : Repository<LineListRevision>, ILineListRevisionRepository
	{
		private readonly LineListDbContext _context;

		public LineListRevisionRepository(LineListDbContext context) : base(context)
		{
		}

		public override async Task<List<LineListRevision>> GetAll()
		{
			return await Db.LineListRevisions.AsNoTracking()
				.Include(b => b.Area)
				.Include(b => b.LineList)
				.Include(b => b.LineListStatus)
				.Include(b => b.LineRevisions)
				.Include(b => b.Specification)
				.Include(b => b.EpCompany)
				.Include(b => b.EpProject)
				.Include(a => a.EpProject.CenovusProject)  // Include CenovusProject first

				.ToListAsync();
		}
        public async Task<List<LineListRevision>> GetAllLineListRevisions()
		{
			return await Db.LineListRevisions.AsNoTracking()
                .Include(b => b.LineListStatus)
                .ToListAsync();

		}
        public async Task<List<LineListRevision>> GetReservedByProjectId(Guid epProjectId)
        {
            return await Db.LineListRevisions
                .AsNoTracking()
                .Where(r => r.EpProjectId == epProjectId
                         && r.LineListStatus.Description == "Reserved")
                .Include(r => r.LineListStatus) // Only needed for filtering
                .ToListAsync();
        }

        public async Task<LineListRevision> GetReservedByLineListId(Guid lineListId)
        {
            return await Db.LineListRevisions
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.LineListId == lineListId);
        }
        public override async Task<LineListRevision> GetById(Guid id)
        {
            return await Db.LineListRevisions.AsNoTracking()
                .Include(b => b.Area)
                .Include(b => b.LineList)
                .Include(b => b.LineListStatus)
                .Include(b => b.LineRevisions)
                .Include(b => b.Specification)
                .Include(b => b.EpCompany)
                .Include(b => b.EpProject)
                .Include(a => a.EpProject.CenovusProject)  // Include CenovusProject first
                .Include(b => b.EpProject.CenovusProject.Facility)
                    .FirstOrDefaultAsync(x => x.Id == id);

            //.ToListAsync();
        }

        public async Task<List<LineListRevision>> GetFilteredLineListRevisions(
	Guid? facilityId, Guid? lineListId, Guid? locationId, Guid? areaId,
	Guid? epId, Guid? projectId, Guid? epProjectId, Guid? statusId,
	bool showDrafts, bool showOnlyActive, string documentNumber,
	string modularId, Guid? projectTypeId)
		{
			var query = Db.LineListRevisions.AsNoTracking()
				.Include(b => b.Area)
				.Include(b => b.LineList)
				.Include(b => b.LineListStatus)
				.Include(b => b.LineRevisions)
				.Include(b => b.Specification)
				.Include(b => b.EpCompany)
				.Include(b => b.EpProject)
				.Include(a => a.EpProject.CenovusProject)
                .Include(b => b.EpProject.CenovusProject.Facility) 
				.Include(b => b.EpProject.CenovusProject.ProjectType) 
				.Include(b => b.Location)
                .AsQueryable(); // Keep as IQueryable for efficient filtering

			if (facilityId.HasValue)
				query = query.Where(l => l.EpProject.CenovusProject.FacilityId == facilityId);
			if (lineListId.HasValue)
				query = query.Where(l => l.LineListId == lineListId);
			if (locationId.HasValue)
				query = query.Where(x => x.LocationId == locationId);
			if (areaId.HasValue)
				query = query.Where(x => x.AreaId == areaId);
			if (epId.HasValue)
				query = query.Where(x => x.EpProject.EpCompanyId == epId);
			if (projectId.HasValue)
				query = query.Where(x => x.EpProject.CenovusProjectId == projectId);
			if (epProjectId.HasValue)
				query = query.Where(x => x.EpProjectId == epProjectId);
			if (statusId.HasValue)
			{
				if (showDrafts)
					query = query.Where(x => x.LineListStatusId == statusId || x.LineListStatus.IsDraftOfId == statusId);
				else
					query = query.Where(x => x.LineListStatusId == statusId);
			}
			if (projectTypeId.HasValue)
				query = query.Where(x => x.EpProject.CenovusProject.ProjectTypeId == projectTypeId);
			if (!string.IsNullOrWhiteSpace(documentNumber))
				query = query.Where(x => x.LineList.DocumentNumber == documentNumber);
			if (showOnlyActive)
				query = query.Where(x => x.IsActive);
			if (!showDrafts)
				query = query.Where(x => x.LineListStatus.IsDraftOf == null);
			if (!string.IsNullOrWhiteSpace(modularId))
				query = query.Where(x => x.LineRevisions.Any(lr => lr.Line.ModularId == modularId));

			return await query.ToListAsync();
		}

        public async Task<List<LineListResultDto>> GetFilteredLineListRevisionsNew(
Guid? facilityId, Guid? lineListId, Guid? locationId, Guid? areaId,
Guid? epCompanyId, Guid? projectId, Guid? epProjectId, Guid? statusId,
bool showDrafts, bool showOnlyActive, string documentNumber,
string modularId, Guid? projectTypeId)
        {
            using var db = new LineListDbContext();

            // Precompute values for IsHighestRev and HasLines
            var highestRevMap = await db.LineLists
                .Select(l => new
                {
                    l.Id,
                    HighestRevId = l.LineListRevisions
                        .OrderByDescending(r => r.LineListStatus.IsHardRevision)
                        .ThenByDescending(r => r.DocumentRevisionSort)
                        .Select(r => r.Id)
                        .FirstOrDefault()
                }).ToDictionaryAsync(x => x.Id, x => x.HighestRevId);

            var hasLinesSet = await db.LineRevisions
                .Select(lr => lr.LineListRevisionId)
                .Distinct()
                .ToHashSetAsync();

            var activeDraftSet = await db.LineListRevisions
                .Where(r => r.IsActive && r.LineListStatus.IsDraftOfId != null)
                .Select(r => r.LineListId)
                .Distinct()
                .ToHashSetAsync();

            var baseQuery = db.LineListRevisions
                .AsNoTracking()
                .Where(x =>
                    (!facilityId.HasValue || x.EpProject.CenovusProject.FacilityId == facilityId) &&
                    (!lineListId.HasValue || x.LineListId == lineListId) &&
                    (!locationId.HasValue || x.LocationId == locationId || x.LineRevisions.Any(r => r.Line.LocationId == locationId)) &&
                    (!areaId.HasValue || x.AreaId == areaId || x.LineRevisions.Any(r => r.AreaId == areaId)) &&
                    (!epCompanyId.HasValue || x.EpProject.EpCompanyId == epCompanyId) &&
                    (!projectId.HasValue || x.EpProject.CenovusProjectId == projectId) &&
                    (!epProjectId.HasValue || x.EpProjectId == epProjectId) &&
                    (!statusId.HasValue ||
                        (showDrafts ?
                            (x.LineListStatusId == statusId || x.LineListStatus.IsDraftOfId == statusId) :
                            x.LineListStatusId == statusId)) &&
                    (!projectTypeId.HasValue || x.EpProject.CenovusProject.ProjectTypeId == projectTypeId) &&
                    (string.IsNullOrWhiteSpace(documentNumber) || x.LineList.Id == new Guid(documentNumber)) &&
                    (!showOnlyActive || x.IsActive) &&
                    (showDrafts || x.LineListStatus.IsDraftOf == null) &&
                    (string.IsNullOrWhiteSpace(modularId) || x.LineRevisions.Any(r => r.Line.ModularId == modularId))
                )
                .Select(p => new LineListResultDto
                {
                    Id = p.Id,
                    AreaName = p.Area.Name,
                    CenovusProjectName = p.EpProject.CenovusProject.Name,
                    Description = p.Description,
                    ModularId = p.LineRevisions
                        .Where(lr => lr.Line.ModularId == modularId)
                        .Select(lr => lr.Line.ModularId)
                        .FirstOrDefault(),
                    DocumentNumber = p.LineList.DocumentNumber,
                    DocumentRevision = p.DocumentRevision,
                    DocumentRevisionSort = p.DocumentRevisionSort,
                    EpCompanyName = p.EpCompany.Name,
                    EpProjectId = p.EpProjectId,
                    EpProjectName = p.EpProject.Name,
                    FacilityId = p.EpProject.CenovusProject.FacilityId,
                    FacilityName = p.EpProject.CenovusProject.Facility.Name,
                    HasLines = hasLinesSet.Contains(p.Id),
                    LineListStatusId = p.LineListStatusId,
                    LineListStatusName = p.LineListStatus.Name,
                    LocationName = p.Location.Name,
                    ProjectTypeName = p.EpProject.CenovusProject.ProjectType.Name,
                    IssuedOn = p.LineListStatus.IsIssuedOfId != null ? p.IssuedOn : (DateTime?)null,
                    SpecificationName = p.Specification.Name,
                    IsDraft = p.LineListStatus.IsDraftOfId != null,
                    IsHardRevision = p.LineListStatus.IsHardRevision,
                    IsIssued = p.LineListStatus.IsIssuedOfId != null,
                    IsHighestRev = highestRevMap.ContainsKey(p.LineListId) && highestRevMap[p.LineListId] == p.Id,
                    HasActiveDrafts = activeDraftSet.Contains(p.LineListId),
                    HasEpCompanyAlpha = p.EpCompany.EpCompanyAlphas.Any(a => a.FacilityId == p.EpProject.CenovusProject.FacilityId),
                    IsEpActive = p.EpCompany.IsActive,
                    NewHasEpCompanyAlpha = p.EpProject.EpCompany.EpCompanyAlphas.Any(a => a.FacilityId == p.EpProject.CenovusProject.FacilityId),
                    NewIsEpActive = p.EpProject.EpCompany.IsActive,
                    NewEpCompanyName = p.EpProject.EpCompany.Name,
                    CreatedOn = p.CreatedOn
                })
                .OrderBy(p => p.DocumentNumber)
                .ThenBy(p => p.DocumentRevisionSort);

            return await baseQuery.ToListAsync();
        }

        public async Task<Guid> GetReservedLineListRevisionIdByProjectId(Guid epProjectId)
        {
            return await Db.LineListRevisions
                .Where(m => m.EpProjectId == epProjectId && m.LineListStatus.Name.ToUpper() == "RESERVED")
                .Select(m => m.Id)
                .FirstOrDefaultAsync();
        }
    }
}