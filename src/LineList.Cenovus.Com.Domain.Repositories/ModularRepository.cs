using LineList.Cenovus.Com.Domain.DataTransferObjects;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Domain.Repositories
{
    public class ModularRepository : Repository<Modular>, IModularRepository
    {
        private readonly LineListDbContext _context;

        public ModularRepository(LineListDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ModularDropDownModel>> GetModularIds(Guid facilityId, Guid projectTypeId, Guid epCompanyId, Guid epProjectId, Guid cenovusProjectId)
        {
            var revs = _context.LineListRevisions.AsQueryable();

            if (facilityId != Guid.Empty)
                revs = revs.Where(m => m.EpProject.CenovusProject.FacilityId == facilityId);

            if (projectTypeId != Guid.Empty)
                revs = revs.Where(m => m.EpProject.CenovusProject.ProjectTypeId == (projectTypeId));

            if (epCompanyId != Guid.Empty)
                revs = revs.Where(m => m.EpCompanyId == (epCompanyId));

            if (epProjectId != Guid.Empty)
                revs = revs.Where(m => m.EpProjectId == (epProjectId));

            if (cenovusProjectId != Guid.Empty)
                revs = revs.Where(m => m.EpProject.CenovusProjectId == (cenovusProjectId));

            var result = await (from a in _context.Lines
                                join h in _context.LineRevisions on a.Id equals h.LineId
                                join c in revs on h.LineListRevisionId equals c.Id
                                where c.EpProject.CenovusProject.FacilityId == (facilityId)
                                      && c.LineList.DocumentNumber != null
                                      && c.EpProject.CenovusProject.ProjectTypeId != null
                                      && c.EpProjectId != null
                                      && c.EpProject.CenovusProjectId != null
                                select a).Where(m => m.ModularId != string.Empty)
                                  .OrderBy(m => m.ModularId)
                                  .Distinct()
                                  .ToListAsync();

            var list = result.Select(item => new ModularDropDownModel(item.ModularId, item.Id.ToString()))
                             .OrderBy(i => i.Name)
                             .ToList();

            var index = 0;
            while (index < list.Count - 1)
            {
                if (list[index].Name == list[index + 1].Name)
                    list.RemoveAt(index);
                else
                    index++;
            }

            return list.OrderBy(m => m.Name).ToList();
        }
    }
}