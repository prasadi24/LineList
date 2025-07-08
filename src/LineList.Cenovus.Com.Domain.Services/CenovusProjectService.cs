using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class CenovusProjectService : ICenovusProjectService
    {
        private readonly ICenovusProjectRepository _cenovusProjectRepository;

        public CenovusProjectService(ICenovusProjectRepository cenovusProjectRepository)
        {
            _cenovusProjectRepository = cenovusProjectRepository;
        }

        public async Task<IEnumerable<CenovusProject>> GetAll()
        {
            return await _cenovusProjectRepository.GetAll();
        }

        public async Task<CenovusProject> GetById(Guid id)
        {
            return await _cenovusProjectRepository.GetById(id);
        }

        public async Task<CenovusProject> Add(CenovusProject cenovusProject)
        {
            if (_cenovusProjectRepository.Search(p => p.Name == cenovusProject.Name).Result.Any())
                return null;

            await _cenovusProjectRepository.Add(cenovusProject);
            return cenovusProject;
        }

        public async Task<CenovusProject> Update(CenovusProject cenovusProject)
        {
            if (_cenovusProjectRepository.Search(p => p.Name == cenovusProject.Name && p.Id != cenovusProject.Id).Result.Any())
                return null;

            await _cenovusProjectRepository.Update(cenovusProject);
            return cenovusProject;
        }

        public async Task<bool> Remove(CenovusProject cenovusProject)
        {
            await _cenovusProjectRepository.Remove(cenovusProject);
            return true;
        }

        public async Task<IEnumerable<CenovusProject>> Search(string searchCriteria)
        {
            return await _cenovusProjectRepository.Search(p => p.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _cenovusProjectRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _cenovusProjectRepository.HasDependencies(id);
        }
    }
}