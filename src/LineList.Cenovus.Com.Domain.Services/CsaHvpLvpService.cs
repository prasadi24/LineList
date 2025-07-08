using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class CsaHvpLvpService : ICsaHvpLvpService
    {
        private readonly ICsaHvpLvpRepository _csaHvpLvpRepository;

        public CsaHvpLvpService(ICsaHvpLvpRepository csaHvpLvpRepository)
        {
            _csaHvpLvpRepository = csaHvpLvpRepository;
        }

        public async Task<IEnumerable<CsaHvpLvp>> GetAll()
        {
            return await _csaHvpLvpRepository.GetAll();
        }

        public async Task<CsaHvpLvp> GetById(Guid id)
        {
            return await _csaHvpLvpRepository.GetById(id);
        }

        public async Task<CsaHvpLvp> Add(CsaHvpLvp csaHvpLvp)
        {
            if (_csaHvpLvpRepository.Search(c => c.Name == csaHvpLvp.Name).Result.Any())
                return null;

            await _csaHvpLvpRepository.Add(csaHvpLvp);
            return csaHvpLvp;
        }

        public async Task<CsaHvpLvp> Update(CsaHvpLvp csaHvpLvp)
        {
            if (_csaHvpLvpRepository.Search(c => c.Name == csaHvpLvp.Name && c.Id != csaHvpLvp.Id).Result.Any())
                return null;

            await _csaHvpLvpRepository.Update(csaHvpLvp);
            return csaHvpLvp;
        }

        public async Task<bool> Remove(CsaHvpLvp csaHvpLvp)
        {
            await _csaHvpLvpRepository.Remove(csaHvpLvp);
            return true;
        }

        public async Task<IEnumerable<CsaHvpLvp>> Search(string searchCriteria)
        {
            return await _csaHvpLvpRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _csaHvpLvpRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _csaHvpLvpRepository.HasDependencies(id);
        }
    }
}