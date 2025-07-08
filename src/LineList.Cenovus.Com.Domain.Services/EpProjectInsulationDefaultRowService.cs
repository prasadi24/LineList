using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpProjectInsulationDefaultRowService : IEpProjectInsulationDefaultRowService
    {
        private readonly IEpProjectInsulationDefaultRowRepository _epProjectInsulationDefaultRowRepository;

        public EpProjectInsulationDefaultRowService(IEpProjectInsulationDefaultRowRepository epProjectInsulationDefaultRowRepository)
        {
            _epProjectInsulationDefaultRowRepository = epProjectInsulationDefaultRowRepository;
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultRow>> GetAll()
        {
            return await _epProjectInsulationDefaultRowRepository.GetAll();
        }

        public async Task<EpProjectInsulationDefaultRow> GetById(Guid id)
        {
            return await _epProjectInsulationDefaultRowRepository.GetById(id);
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultRow>> GetByInsulationDefaultId(Guid id)
        {
            return await _epProjectInsulationDefaultRowRepository.GetByInsulationDefaultId(id);
        }

        public async Task<EpProjectInsulationDefaultRow> Add(EpProjectInsulationDefaultRow epProjectInsulationDefaultRow)
        {
            await _epProjectInsulationDefaultRowRepository.Add(epProjectInsulationDefaultRow);
            return epProjectInsulationDefaultRow;
        }

        public async Task<EpProjectInsulationDefaultRow> Update(EpProjectInsulationDefaultRow epProjectInsulationDefaultRow)
        {
            await _epProjectInsulationDefaultRowRepository.Update(epProjectInsulationDefaultRow);
            return epProjectInsulationDefaultRow;
        }

        public async Task<bool> Remove(EpProjectInsulationDefaultRow epProjectInsulationDefaultRow)
        {
            await _epProjectInsulationDefaultRowRepository.Remove(epProjectInsulationDefaultRow);
            return true;
        }

        public void Dispose()
        {
            _epProjectInsulationDefaultRowRepository?.Dispose();
        }
    }
}