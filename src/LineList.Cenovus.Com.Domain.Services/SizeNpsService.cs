using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class SizeNpsService : ISizeNpsService
    {
        private readonly ISizeNpsRepository _sizeNpsRepository;

        public SizeNpsService(ISizeNpsRepository sizeNpsRepository)
        {
            _sizeNpsRepository = sizeNpsRepository;
        }

        public async Task<IEnumerable<SizeNps>> GetAll()
        {
            return await _sizeNpsRepository.GetAll();
        }

        public async Task<SizeNps> GetById(Guid id)
        {
            return await _sizeNpsRepository.GetById(id);
        }

        public async Task<SizeNps> Add(SizeNps sizeNps)
        {
            // Prevent adding a duplicate SizeNps entry based on Name
            if (_sizeNpsRepository.Search(c => c.Name == sizeNps.Name).Result.Any())
                return null;

            await _sizeNpsRepository.Add(sizeNps);
            return sizeNps;
        }

        public async Task<SizeNps> Update(SizeNps sizeNps)
        {
            // Prevent updating to a duplicate SizeNps entry based on Name
            if (_sizeNpsRepository.Search(c => c.Name == sizeNps.Name && c.Id != sizeNps.Id).Result.Any())
                return null;

            await _sizeNpsRepository.Update(sizeNps);
            return sizeNps;
        }

        public async Task<bool> Remove(SizeNps sizeNps)
        {
            await _sizeNpsRepository.Remove(sizeNps);
            return true;
        }

        public async Task<IEnumerable<SizeNps>> Search(string searchCriteria)
        {
            return await _sizeNpsRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _sizeNpsRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _sizeNpsRepository.HasDependencies(id);
        }
    }
}