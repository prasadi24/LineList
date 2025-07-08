using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class EpProjectInsulationDefaultDetailService : IEpProjectInsulationDefaultDetailService
    {
        private readonly IEpProjectInsulationDefaultDetailRepository _epProjectInsulationDefaultDetailRepository;

        public EpProjectInsulationDefaultDetailService(IEpProjectInsulationDefaultDetailRepository epProjectInsulationDefaultDetailRepository)
        {
            _epProjectInsulationDefaultDetailRepository = epProjectInsulationDefaultDetailRepository;
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetAll()
        {
            return await _epProjectInsulationDefaultDetailRepository.GetAll();
        }

        public async Task<EpProjectInsulationDefaultDetail> GetById(Guid id)
        {
            return await _epProjectInsulationDefaultDetailRepository.GetById(id);
        }

        public async Task<EpProjectInsulationDefaultDetail> Add(EpProjectInsulationDefaultDetail epProjectInsulationDefaultDetail)
        {
            // Example condition for checking before adding
            if (_epProjectInsulationDefaultDetailRepository.Search(c => c.Id == epProjectInsulationDefaultDetail.Id).Result.Any())
                return null;

            await _epProjectInsulationDefaultDetailRepository.Add(epProjectInsulationDefaultDetail);
            return epProjectInsulationDefaultDetail;
        }

        public async Task<EpProjectInsulationDefaultDetail> Update(EpProjectInsulationDefaultDetail epProjectInsulationDefaultDetail)
        {
            // Example condition for checking before updating
            if (_epProjectInsulationDefaultDetailRepository.Search(c => c.Id == epProjectInsulationDefaultDetail.Id && c.Id != epProjectInsulationDefaultDetail.Id).Result.Any())
                return null;

            await _epProjectInsulationDefaultDetailRepository.Update(epProjectInsulationDefaultDetail);
            return epProjectInsulationDefaultDetail;
        }

        public async Task<bool> Remove(EpProjectInsulationDefaultDetail epProjectInsulationDefaultDetail)
        {
            await _epProjectInsulationDefaultDetailRepository.Remove(epProjectInsulationDefaultDetail);
            return true;
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetByInsulationDefaultId(Guid id)
        {
            return await _epProjectInsulationDefaultDetailRepository.GetByInsulationDefaultId(id);
        }

        public async Task<IEnumerable<EpProjectInsulationDefaultDetail>> GetByInsulationDefaultId(Guid rowId, Guid ColumnId)
        {
            return await _epProjectInsulationDefaultDetailRepository.GetByInsulationDefaultId(rowId, ColumnId);
        }


        public void Dispose()
        {
            _epProjectInsulationDefaultDetailRepository?.Dispose();
        }
    }
}