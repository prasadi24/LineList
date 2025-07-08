using LineList.Cenovus.Com.API.DataTransferObjects.Line;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class LineRevisionRepository : Repository<LineRevision>, ILineRevisionRepository
    {
        private readonly LineListDbContext _context;

        public LineRevisionRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public override async Task<List<LineRevision>> GetAll()
        {
            var lines = await Db.LineRevisions
                                    .Include(b => b.Line)
                                    .Include(b => b.LineListRevision)
                                    .Include(b => b.LineListRevision.LineListStatus)
                                    .ToListAsync();

            return lines;
        }

        public async Task<bool> HasRevisionsForProject(Guid epProjectId)
        {
            return await Db.LineRevisions
                           .AnyAsync(m => m.LineListRevision.EpProjectId == epProjectId);
        }
        public async Task<List<LineRevision>> GetByIds(List<Guid> ids)
        {
            return await Db.LineRevisions
                .Include(lr => lr.Line)
                .Where(lr => ids.Contains(lr.Id))
                .ToListAsync();
        }
        public async Task<List<LineRevision>> GetCheckOutLines(Guid[] ids, string userName)
        {
            var lineRevisions = await Db.LineRevisions
                .Where(m => (!m.IsCheckedOut || m.CheckedOutBy == userName) && ids.Contains(m.Id))
                .ToListAsync();

            return lineRevisions;
        }

        public async Task<List<LineRevision>> GetCheckInLines(Guid[] ids, string username, bool isAdmin)
        {
            var lineRevisions = await Db.LineRevisions
                .Where(m => m.IsCheckedOut && ids.Contains(m.Id) && (isAdmin || m.CheckedOutBy == username))
                .ToListAsync();
            return lineRevisions;
        }

        public async Task<Guid[]> GetDiscardableIds(Guid[] possibles, string userName)
        {
            var lineRevisions = await Db.LineRevisions
                .Where(m => possibles.Contains(m.Id) && m.CheckedOutBy == userName)
                .Where(m => !Db.LineRevisions.Any(z =>
                             z.LineId == m.LineId &&
                             z.LineListRevision.LineListStatus.IsIssuedOfId != null &&
                             z.LineListRevision.LineListId == m.LineListRevision.LineListId))
                .Where(m => !Db.LineRevisions.Any(z =>
                             z.Line.ChildNumber != 0 &&
                             z.Line.LocationId == m.Line.LocationId &&
                             z.Line.CommodityId == m.Line.CommodityId &&
                             z.Line.SequenceNumber == m.Line.SequenceNumber &&
                             z.LineListRevision.LineListId == m.LineListRevision.LineListId))
                .Select(m => m.Id)
                .ToArrayAsync();

            return lineRevisions;
        }
        public override async Task<LineRevision> GetById(Guid Id)
        {
            var line = await Db.LineRevisions.AsNoTracking()
                                    .Where(lr => lr.Id == Id)
                                    .Include(b => b.Line)
                                    .Include(b => b.LineListRevision)
                                    .Include(b => b.LineListRevision.LineListStatus)
                                    .Include(b => b.SizeNpsPipe)
                                    .Include(b => b.PipeSpecification)
                                    .Include(b => b.LineRevisionOperatingModes)
                                    .Include(b => b.Area)
                                    .Include(b => b.LineStatus)
                                    .Include(b => b.Specification)
                                    .Include(b => b.Line.Location)
                                    .Include(b => b.Line.Commodity)
                                    .Include(b => b.LineListRevision.EpProject)
                                    .FirstOrDefaultAsync();

            return line;
        }
        public async Task<List<LineRevision>> GetByEpProjectId(Guid epProjectId)
        {
            var lineRevisions = await Db.LineRevisions
                .Include(lr => lr.LineListRevision)
                 .ThenInclude(lr => lr.LineListStatus)
                 .Include(lr => lr.LineStatus)
                                    .Where(lr => lr.LineListRevision != null && lr.LineListRevision.EpProjectId == epProjectId)
                                    .ToListAsync();
            //if (lineRevisions == null || !lineRevisions.Any())
            //    _logger.LogInformation("No LineRevisions found for EpProjectId {EpProjectId}", epProjectId);

            return lineRevisions;
        }
        public async Task<List<LineRevision>> GetByLineId(Guid lineId)
        {
            var lines = await Db.LineRevisions
                                    .Where(lr => lr.LineId == lineId)
                                    .Include(b => b.Line)
                                    .Include(b => b.LineStatus)
                                    .Include(b => b.Line.Location)
                                    .Include(b => b.Line.Commodity)
                                    .Include(b => b.SizeNpsPipe)
                                    .Include(b => b.LineRevisionOperatingModes)
                                    .Include(b => b.LineRevisionSegments)
                                        .ThenInclude(b => b.SegmentType)
                                    .ToListAsync();

            return lines;
        }

        public async Task<List<LineRevision>> GetByLineListRevisionId(Guid lineListRevisionId)
        {
            var lineRevisions = await Db.LineRevisions
                                    .Where(lr => lr.LineListRevisionId == lineListRevisionId)
                                    .Include(b => b.Area)
                                    .Include(b => b.Line)
                                    .Include(b => b.LineStatus)
                                    .Include(b => b.Line.Location)
                                    .Include(b => b.Line.Commodity)
                                    .Include(b => b.Specification)
                                    .Include(b => b.LineListRevision)
                                    .Include(b => b.LineListRevision.EpProject)
                                    .Include(b => b.LineListRevision.LineList)
                                    .Include(b => b.LineListRevision.LineListStatus)
                                    .Include(b => b.SizeNpsPipe)
                                    .Include(b => b.PipeSpecification)
                                    .ToListAsync();

            return lineRevisions;
        }

        public async Task<bool> ExistLineRevisions(Guid revisionId, Guid locationId, Guid commodityId, string sequenceNumber, int? childNumber)
        {
            return await Db.LineRevisions.AnyAsync(m =>
                m.LineListRevisionId == revisionId &&
                m.Line.LocationId == locationId &&
                m.Line.CommodityId == commodityId &&
                m.Line.SequenceNumber == sequenceNumber &&
                m.Line.ChildNumber == childNumber);
        }

        public async Task<List<LineResultDto>> GetFilteredLineRevisions(
     Guid? facilityId,
     Guid? specificationId,
     Guid? locationId,
     Guid? commodityId,
     Guid? areaId,
     Guid? cenovusProjectId,
     Guid? epProjectId,
     Guid? pipeSpecificationId,
     Guid? lineStatusId,
     bool showDrafts,
     bool showOnlyActive,
     string documentNumber,
     string modularId,
     string sequenceNumber,
     Guid? projectTypeId)
        {
            // start building the base query
            var query = Db.LineRevisions
                          .AsNoTracking()
                          .AsQueryable();

            // apply all your filters
            if (facilityId.HasValue)
                query = query.Where(x => x.Line.Location.FacilityId == facilityId);

            if (locationId.HasValue)
                query = query.Where(x =>
                     x.Line.LocationId == locationId ||
                     x.LineListRevision.LocationId == locationId);

            if (areaId.HasValue)
                query = query.Where(x =>
                     x.AreaId == areaId ||
                     x.LineListRevision.AreaId == areaId);

            if (cenovusProjectId.HasValue)
                query = query.Where(x =>
                     x.LineListRevision.EpProject.CenovusProjectId == cenovusProjectId);
            if (projectTypeId.HasValue)
                query = query.Where(x =>
                    x.LineListRevision.EpProject.ProjectTypeId == projectTypeId);


            if (epProjectId.HasValue)
                query = query.Where(x =>
                     x.LineListRevision.EpProjectId == epProjectId);

            if (specificationId.HasValue)
                query = query.Where(x =>
                     x.SpecificationId == specificationId ||
                     x.LineListRevision.SpecificationId == specificationId);

            if (pipeSpecificationId.HasValue)
                query = query.Where(x =>
                     x.PipeSpecificationId == pipeSpecificationId);

            if (lineStatusId.HasValue)
                query = query.Where(x =>
                     x.LineStatusId == lineStatusId);

            if (commodityId.HasValue)
                query = query.Where(x =>
                     x.Line.CommodityId == commodityId);

            if (!string.IsNullOrWhiteSpace(documentNumber))
                query = query.Where(x =>
                     x.LineListRevision.LineList.DocumentNumber == documentNumber);

            if (!string.IsNullOrWhiteSpace(modularId))
                query = query.Where(x =>
                     x.Line.ModularId == modularId);

            if (!string.IsNullOrWhiteSpace(sequenceNumber))
                query = query.Where(x =>
                     x.Line.SequenceNumber == sequenceNumber);

            if (showOnlyActive)
                query = query.Where(x => x.IsActive);

            if (!showDrafts)
                query = query.Where(x =>
                     x.LineListRevision.LineListStatus.IsDraftOfId == null);

            // now project into your DTO, including parent/child logic
            var result = await query
                .Select(x => new LineResultDto
                {
                    Id = x.Id,
                    LineId = x.LineId,
                    SequenceNumber = x.Line.SequenceNumber,
                    ModularId = x.Line.ModularId,
                    CommodityId = x.Line.CommodityId,
                    LocationId = x.Line.LocationId,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,

                    DocumentNumber = x.LineListRevision.LineList.DocumentNumber,
                    DocumentRevision = x.Revision,
                    DocumentLineListRevision = x.LineListRevision.DocumentRevision,

                    LineListStatusName = x.LineListRevision.LineListStatus.Name,
                    LineStatusName = x.LineStatus.Name,

                    SpecificationName = x.Specification.Name,
                    PipeSpecificationName = x.PipeSpecification.Name,
                    AreaName = x.Area.Name,
                    LocationName = x.Line.Location.Name,
                    CommoditiyName = x.Line.Commodity.Name,
                    SizeNpsName = x.SizeNpsPipe.Name,

                    ParentChild = x.Line.ChildNumber != 0
                        ? "C"
                        : Db.Lines.Any(l =>
                              l.LocationId == x.Line.LocationId &&
                              l.CommodityId == x.Line.CommodityId &&
                              l.SequenceNumber == x.Line.SequenceNumber &&
                              l.ChildNumber != 0)
                          ? "p"
                          : string.Empty,

                    LineListRevisionId = x.LineListRevisionId,
                    LineRevision = x.Revision
                })
                .ToListAsync();

            return result;
        }

        public async Task<LineRevision> UpdateStatus(List<KeyValuePair<Guid, bool>> activeSettings)
        {
            using (LineListDbContext db = new LineListDbContext())
            {
                foreach (var item in activeSettings)
                {
                    var rev = db.LineRevisions.FirstOrDefault(r => r.Id == item.Key);
                    if (rev != null)
                    {
                        if (rev.IsActive != item.Value)
                        {
                            rev.IsActive = item.Value;

                        }
                    }
                }
                db.SaveChanges();
                return new LineRevision();
            }
        }

        public async Task<bool> AnyCheckedOutAsync(Guid lineListRevisionId)
        {
            return await _context.LineRevisions
                .AsNoTracking()
                .Where(lr => lr.LineListRevisionId == lineListRevisionId && lr.IsCheckedOut)
                .AnyAsync();
        }


    }
}