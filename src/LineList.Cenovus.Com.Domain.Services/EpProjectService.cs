using LineList.Cenovus.Com.API.DataTransferObjects.EpProject;
using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpProjectService : IEpProjectService
    {
        private readonly IEpProjectRepository _epProjectRepository;

        public EpProjectService(IEpProjectRepository epProjectRepository)
        {
            _epProjectRepository = epProjectRepository;
        }

        public async Task<IEnumerable<EpProject>> GetAll()
        {
            return await _epProjectRepository.GetAll();
        }

        public async Task<EpProject> GetById(Guid id)
        {
            return await _epProjectRepository.GetById(id);
        }
        public async Task<IEnumerable<EpProjectResultDto>> GetDataSource(bool showActive)
        {
            return await _epProjectRepository.GetDataSource(showActive);
        }
        public async Task<EpProject> Add(EpProject epProject)
        {
            if (_epProjectRepository.Search(c => c.Name == epProject.Name).Result.Any())
                return null;

            await _epProjectRepository.Add(epProject);
            return epProject;
        }

        public async Task<EpProject> Update(EpProject epProject)
        {
            if (_epProjectRepository.Search(c => c.Name == epProject.Name && c.Id != epProject.Id).Result.Any())
                return null;

            await _epProjectRepository.Update(epProject);
            return epProject;
        }

        public async Task<bool> Remove(EpProject epProject)
        {
            await _epProjectRepository.Remove(epProject);
            return true;
        }

        public async Task<IEnumerable<EpProject>> Search(string searchCriteria)
        {
            return await _epProjectRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _epProjectRepository?.Dispose();
        }
    }
}