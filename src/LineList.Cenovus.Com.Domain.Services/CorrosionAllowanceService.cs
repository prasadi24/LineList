using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class CorrosionAllowanceService : ICorrosionAllowanceService
    {
        private readonly ICorrosionAllowanceRepository _corrosionAllowanceRepository;

        public CorrosionAllowanceService(ICorrosionAllowanceRepository corrosionAllowanceRepository)
        {
            _corrosionAllowanceRepository = corrosionAllowanceRepository;
        }

        public async Task<IEnumerable<CorrosionAllowance>> GetAll()
        {
            return await _corrosionAllowanceRepository.GetAll();
        }

        public async Task<CorrosionAllowance> GetById(Guid id)
        {
            return await _corrosionAllowanceRepository.GetById(id);
        }

        public async Task<CorrosionAllowance> Add(CorrosionAllowance corrosionAllowance)
        {
            if (_corrosionAllowanceRepository.Search(c => c.Name == corrosionAllowance.Name).Result.Any())
                return null;

            await _corrosionAllowanceRepository.Add(corrosionAllowance);
            return corrosionAllowance;
        }

        public async Task<CorrosionAllowance> Update(CorrosionAllowance corrosionAllowance)
        {
            if (_corrosionAllowanceRepository.Search(c => c.Name == corrosionAllowance.Name && c.Id != corrosionAllowance.Id).Result.Any())
                return null;

            await _corrosionAllowanceRepository.Update(corrosionAllowance);
            return corrosionAllowance;
        }

        public async Task<bool> Remove(CorrosionAllowance corrosionAllowance)
        {
            await _corrosionAllowanceRepository.Remove(corrosionAllowance);
            return true;
        }

        public async Task<IEnumerable<CorrosionAllowance>> Search(string searchCriteria)
        {
            return await _corrosionAllowanceRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _corrosionAllowanceRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _corrosionAllowanceRepository.HasDependencies(id);
        }
    }
}