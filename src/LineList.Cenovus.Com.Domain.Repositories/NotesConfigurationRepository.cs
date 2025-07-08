using LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace LineList.Cenovus.Com.Infrastructure.Repositories
{
    public class NotesConfigurationRepository : INotesConfigurationRepository
    {
        private readonly LineListDbContext _context;

        public NotesConfigurationRepository(LineListDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NotesConfiguration>> GetAll()
        {
            //try { 
            return await _context.NotesConfigurations.AsNoTracking()
                .OrderBy(n => n.FacilityId)
                .ToListAsync();
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}
        }

        public async Task<NotesConfiguration> GetById(Guid id)
        {
            return await _context.NotesConfigurations.FindAsync(id);
        }

        public async Task Add(NotesConfiguration entity)
        {
            _context.NotesConfigurations.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Update(NotesConfiguration entity)
        {
            _context.NotesConfigurations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Remove(NotesConfiguration entity)
        {
            _context.NotesConfigurations.Remove(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<NotesConfigurationResultDto>> GetAllWithNames()
        {
            var query = from n in _context.NotesConfigurations.AsNoTracking()
                        join f in _context.Facilities.AsNoTracking()
                            on n.FacilityId equals f.Id
                        join s in _context.Specifications.AsNoTracking()
                            on n.SpecificationId equals s.Id
                        orderby f.Name, s.Name
                        select new NotesConfigurationResultDto
                        {
                            Id = n.Id, 
                            FacilityId = n.FacilityId, 
                            SpecificationId = n.SpecificationId, 
                            FacilityName = f.Name,
                            SpecificationName = s.Name,
                            FileName = n.FileName,
                            FileData = n.FileData,
                            UploadedBy = n.UploadedBy,
                            UploadedOn = n.UploadedOn
                        };

            return await query.ToListAsync();
        }
    }
}
