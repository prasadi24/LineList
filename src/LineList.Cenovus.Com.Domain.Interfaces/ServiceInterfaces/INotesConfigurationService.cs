using LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface INotesConfigurationService
    {
        Task<IEnumerable<NotesConfiguration>> GetAll();
        Task<NotesConfiguration> GetById(Guid id);
        Task Add(NotesConfiguration config);
        Task Update(NotesConfiguration config);
        Task Remove(NotesConfiguration config);

        Task<IEnumerable<NotesConfigurationResultDto>> GetAllWithNames();
    }
}
