using LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface INotesConfigurationRepository
    {
        Task<IEnumerable<NotesConfiguration>> GetAll();
        Task<NotesConfiguration> GetById(Guid id);
        Task Add(NotesConfiguration entity);
        Task Update(NotesConfiguration entity);
        Task Remove(NotesConfiguration entity);

        Task<IEnumerable<NotesConfigurationResultDto>> GetAllWithNames();

    }
}
