using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class InsulationDefaultRowService : IInsulationDefaultRowService
    {
        private readonly IInsulationDefaultRowRepository _insulationDefaultRowRepository;

        public InsulationDefaultRowService(IInsulationDefaultRowRepository insulationDefaultRowRepository)
        {
            _insulationDefaultRowRepository = insulationDefaultRowRepository;
        }

        public async Task<IEnumerable<InsulationDefaultRow>> GetAll()
        {
            return await _insulationDefaultRowRepository.GetAll();
        }

        public async Task<InsulationDefaultRow> GetById(Guid id)
        {
            return await _insulationDefaultRowRepository.GetById(id);
        }

        public async Task<IEnumerable<InsulationDefaultRow>> GetByInsulationDefaultId(Guid id)
        {
            return await _insulationDefaultRowRepository.GetByInsulationDefaultId(id);
        }

        public async Task<InsulationDefaultRow> Add(InsulationDefaultRow insulationDefaultRow)
        {
            if (_insulationDefaultRowRepository.Search(c => c.Id == insulationDefaultRow.Id).Result.Any())
                return null;

            await _insulationDefaultRowRepository.Add(insulationDefaultRow);
            return insulationDefaultRow;
        }

        public async Task<InsulationDefaultRow> Update(InsulationDefaultRow insulationDefaultRow)
        {
            if (_insulationDefaultRowRepository.Search(c => c.InsulationDefault == insulationDefaultRow.InsulationDefault && c.Id != insulationDefaultRow.Id).Result.Any())
                return null;

            await _insulationDefaultRowRepository.Update(insulationDefaultRow);
            return insulationDefaultRow;
        }

        public async Task<bool> Remove(InsulationDefaultRow insulationDefaultRow)
        {
            await _insulationDefaultRowRepository.Remove(insulationDefaultRow);
            return true;
        }

        public void Dispose()
        {
            _insulationDefaultRowRepository?.Dispose();
        }
    }
}