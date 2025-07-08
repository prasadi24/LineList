using LineList.Cenovus.Com.API.DataTransferObjects.EpProject;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class EpProjectRepository : Repository<EpProject>, IEpProjectRepository
    {
        private readonly LineListDbContext _context;

        public EpProjectRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }
        public override async Task<List<EpProject>> GetAll()
        {
            return await Db.EpProjects.AsNoTracking()
                .Include(b => b.CenovusProject)
                .ToListAsync();
        }
        public async Task<List<EpProjectResultDto>> GetDataSource(bool showActive)
        {
            var query = Db.EpProjects.AsNoTracking().AsQueryable();

            if (showActive)
                query = query.Where(x => x.IsActive == true); 

            
            var results = await (
                        from ep in query
                        join facility in Db.Facilities on ep.FacilityId equals facility.Id into facilityJoin
                        from fallbackFacility in facilityJoin.DefaultIfEmpty()
                        join projectType in Db.ProjectTypes on ep.ProjectTypeId equals projectType.Id into projectTypeJoin
                        from fallbackProjectType in projectTypeJoin.DefaultIfEmpty()
                        select new EpProjectResultDto
                        {
                            CenovusProjectName = ep.CenovusProject != null ? ep.CenovusProject.Name : null,
                            Description = ep.Description,
                            EpCompanyName = ep.EpCompany.Name,
                            EpCompanyId = ep.EpCompanyId,
                            FacilityName = ep.CenovusProject != null
                                ? ep.CenovusProject.Facility.Name
                                : fallbackFacility.Name,
                            Id = ep.Id,
                            Name = ep.Name,
                            IsActive = ep.IsActive,
                            ProjectTypeName = ep.CenovusProject != null
                                ? ep.CenovusProject.ProjectType.Name
                                : fallbackProjectType.Name,
                            CreatedBy = ep.CreatedBy,
                            CreatedOn = ep.CreatedOn,
                            ModifiedBy = ep.ModifiedBy,
                            ModifiedOn = ep.ModifiedOn,
                            SortOrder = ep.SortOrder,
                            Notes = ep.Notes,
                            CopyInsulationTableDefaultsEpProjectId = ep.CopyInsulationTableDefaultsEpProjectId
                        })
                        .AsNoTracking()
                        .ToListAsync();
            return results;
        }

        public override async Task<EpProject> GetById(Guid id)
        {
            return await _context.EpProjects
                            .Include(b => b.CenovusProject)
                            .Include(b => b.EpCompany)
                             .Where(d => d.Id == id)
                             .FirstOrDefaultAsync();
        }
    }
}