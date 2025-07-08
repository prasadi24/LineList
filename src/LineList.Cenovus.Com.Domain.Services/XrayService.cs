using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class XrayService : IXrayService
    {
        private readonly IXrayRepository _xrayRepository;

        public XrayService(IXrayRepository xrayRepository)
        {
            _xrayRepository = xrayRepository;
        }

        public async Task<IEnumerable<Xray>> GetAll()
        {
            return await _xrayRepository.GetAll();
        }

        public async Task<Xray> GetById(Guid id)
        {
            return await _xrayRepository.GetById(id);
        }

        public async Task<Xray> Add(Xray xray)
        {
            if (_xrayRepository.Search(c => c.Name == xray.Name).Result.Any())
                return null;

            await _xrayRepository.Add(xray);
            return xray;
        }

        public async Task<Xray> Update(Xray xray)
        {
            await _xrayRepository.Update(xray);
            return xray;
        }

        public async Task<bool> Remove(Xray xray)
        {
            await _xrayRepository.Remove(xray);
            return true;
        }

        public async Task<IEnumerable<Xray>> Search(string searchCriteria)
        {
            return await _xrayRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _xrayRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _xrayRepository.HasDependencies(id);
        }
    }
}