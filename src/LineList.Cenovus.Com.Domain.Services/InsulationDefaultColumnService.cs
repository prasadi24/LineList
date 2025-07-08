using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class InsulationDefaultColumnService : IInsulationDefaultColumnService
    {
        private readonly IInsulationDefaultColumnRepository _insulationDefaultColumnRepository;

        public InsulationDefaultColumnService(IInsulationDefaultColumnRepository insulationDefaultColumnRepository)
        {
            _insulationDefaultColumnRepository = insulationDefaultColumnRepository;
        }

        public async Task<IEnumerable<InsulationDefaultColumn>> GetAll()
        {
            return await _insulationDefaultColumnRepository.GetAll();
        }

        public async Task<InsulationDefaultColumn> GetById(Guid id)
        {
            return await _insulationDefaultColumnRepository.GetById(id);
        }

        public async Task<IEnumerable<InsulationDefaultColumn>> GetByInsulationDefaultId(Guid id)
        {
            return await _insulationDefaultColumnRepository.GetByInsulationDefaultId(id);
        }

        public async Task<InsulationDefaultColumn> Add(InsulationDefaultColumn insulationDefaultColumn)
        {
            if (_insulationDefaultColumnRepository.Search(c => c.Id == insulationDefaultColumn.Id).Result.Any())
                return null;

            await _insulationDefaultColumnRepository.Add(insulationDefaultColumn);
            return insulationDefaultColumn;
        }

        public async Task<InsulationDefaultColumn> Update(InsulationDefaultColumn insulationDefaultColumn)
        {
            if (_insulationDefaultColumnRepository.Search(c => c.Id == insulationDefaultColumn.Id && c.Id != insulationDefaultColumn.Id).Result.Any())
                return null;

            await _insulationDefaultColumnRepository.Update(insulationDefaultColumn);
            return insulationDefaultColumn;
        }

        public async Task<bool> Remove(InsulationDefaultColumn insulationDefaultColumn)
        {
            await _insulationDefaultColumnRepository.Remove(insulationDefaultColumn);
            return true;
        }

        //public async Task<IEnumerable<InsulationDefaultColumn>> Search(string searchCriteria)
        //{
        //    return await _insulationDefaultColumnRepository.Search(c => c.Name.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _insulationDefaultColumnRepository?.Dispose();
        }
    }
}