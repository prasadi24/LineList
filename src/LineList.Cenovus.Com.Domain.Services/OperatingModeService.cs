using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class OperatingModeService : IOperatingModeService
    {
        private readonly IOperatingModeRepository _operatingModeRepository;

        public OperatingModeService(IOperatingModeRepository operatingModeRepository)
        {
            _operatingModeRepository = operatingModeRepository;
        }

        public async Task<IEnumerable<OperatingMode>> GetAll()
        {
            return await _operatingModeRepository.GetAll();
        }

        public async Task<OperatingMode> GetById(Guid id)
        {
            return await _operatingModeRepository.GetById(id);
        }
        public async Task<OperatingMode?> GetOperatingModeForPrimary()
        {
            return await _operatingModeRepository.GetOperatingModeForPrimary();
        }

        public async Task<OperatingMode> Add(OperatingMode operatingMode)
        {
            if (_operatingModeRepository.Search(c => c.Name == operatingMode.Name).Result.Any())
                return null;

            await _operatingModeRepository.Add(operatingMode);
            return operatingMode;
        }

        public async Task<OperatingMode> Update(OperatingMode operatingMode)
        {
            if (_operatingModeRepository.Search(c => c.Name == operatingMode.Name && c.Id != operatingMode.Id).Result.Any())
                return null;

            await _operatingModeRepository.Update(operatingMode);
            return operatingMode;
        }

        public async Task<bool> Remove(OperatingMode operatingMode)
        {
            await _operatingModeRepository.Remove(operatingMode);
            return true;
        }

        public async Task<IEnumerable<OperatingMode>> Search(string searchCriteria)
        {
            return await _operatingModeRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _operatingModeRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _operatingModeRepository.HasDependencies(id);
        }
    }
}