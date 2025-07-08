using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class InsulationDefaultDetailService : IInsulationDefaultDetailService
    {
        private readonly IInsulationDefaultDetailRepository _insulationDefaultDetailRepository;

        public InsulationDefaultDetailService(IInsulationDefaultDetailRepository insulationDefaultDetailRepository)
        {
            _insulationDefaultDetailRepository = insulationDefaultDetailRepository;
        }

        public async Task<IEnumerable<InsulationDefaultDetail>> GetAll()
        {
            return await _insulationDefaultDetailRepository.GetAll();
        }

        public async Task<InsulationDefaultDetail> GetById(Guid id)
        {
            return await _insulationDefaultDetailRepository.GetById(id);
        }

        public async Task<InsulationDefaultDetail> Add(InsulationDefaultDetail insulationDefaultDetail)
        {
            await _insulationDefaultDetailRepository.Add(insulationDefaultDetail);
            return insulationDefaultDetail;
        }

        public async Task<InsulationDefaultDetail> Update(InsulationDefaultDetail insulationDefaultDetail)
        {
            await _insulationDefaultDetailRepository.Update(insulationDefaultDetail);
            return insulationDefaultDetail;
        }

        public async Task<bool> Remove(InsulationDefaultDetail insulationDefaultDetail)
        {
            await _insulationDefaultDetailRepository.Remove(insulationDefaultDetail);
            return true;
        }

        public async Task<IEnumerable<InsulationDefaultDetail>> GetByInsulationDefaultId(Guid rowId, Guid ColumnId)
        {
            return await _insulationDefaultDetailRepository.GetByInsulationDefaultId(rowId, ColumnId);
        }

        public void Dispose()
        {
            _insulationDefaultDetailRepository?.Dispose();
        }
    }
}