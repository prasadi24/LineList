using LineList.Cenovus.Com.API.DataTransferObjects.NotesConfiguration;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class NotesConfigurationService : INotesConfigurationService
    {
        private readonly INotesConfigurationRepository _notesRepository;

        public NotesConfigurationService(INotesConfigurationRepository notesRepository)
        {
            _notesRepository = notesRepository ?? throw new ArgumentNullException(nameof(notesRepository));
        }

        public async Task<IEnumerable<NotesConfiguration>> GetAll() => await _notesRepository.GetAll();

        public async Task<NotesConfiguration> GetById(Guid id) => await _notesRepository.GetById(id);

        public async Task Add(NotesConfiguration config) => await _notesRepository.Add(config);

        public async Task Update(NotesConfiguration config) => await _notesRepository.Update(config);

        public async Task Remove(NotesConfiguration config) => await _notesRepository.Remove(config);

        public Task<IEnumerable<NotesConfigurationResultDto>> GetAllWithNames() => _notesRepository.GetAllWithNames();
    }
}
