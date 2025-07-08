using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpProjectInsulationDefaultColumnService : IEpProjectInsulationDefaultColumnService
    {
        private readonly IEpProjectInsulationDefaultColumnRepository _epProjectInsulationDefaultColumnRepository;

        public EpProjectInsulationDefaultColumnService(IEpProjectInsulationDefaultColumnRepository epProjectInsulationDefaultColumnRepository)
        {
            _epProjectInsulationDefaultColumnRepository = epProjectInsulationDefaultColumnRepository;
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultColumn>> GetAll()
        {
            return await _epProjectInsulationDefaultColumnRepository.GetAll();
        }

        public async Task<EpProjectInsulationDefaultColumn> GetById(Guid id)
        {
            return await _epProjectInsulationDefaultColumnRepository.GetById(id);
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultColumn>> GetByInsulationDefaultId(Guid id)
        {
            return await _epProjectInsulationDefaultColumnRepository.GetByInsulationDefaultId(id);
        }
        public async Task<EpProjectInsulationDefaultColumn> Add(EpProjectInsulationDefaultColumn epProjectInsulationDefaultColumn)
        {
            await _epProjectInsulationDefaultColumnRepository.Add(epProjectInsulationDefaultColumn);
            return epProjectInsulationDefaultColumn;
        }

        public async Task<EpProjectInsulationDefaultColumn> Update(EpProjectInsulationDefaultColumn epProjectInsulationDefaultColumn)
        {
            await _epProjectInsulationDefaultColumnRepository.Update(epProjectInsulationDefaultColumn);
            return epProjectInsulationDefaultColumn;
        }

        public async Task<bool> Remove(EpProjectInsulationDefaultColumn epProjectInsulationDefaultColumn)
        {
            await _epProjectInsulationDefaultColumnRepository.Remove(epProjectInsulationDefaultColumn);
            return true;
        }

        public void Dispose()
        {
            _epProjectInsulationDefaultColumnRepository?.Dispose();
        }
    }
}