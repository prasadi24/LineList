using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class CommodityService : ICommodityService
    {
        private readonly ICommodityRepository _commodityRepository;

        public CommodityService(ICommodityRepository commodityRepository)
        {
            _commodityRepository = commodityRepository;
        }

        public async Task<IEnumerable<Commodity>> GetAll()
        {
            return await _commodityRepository.GetAll();
        }

        public async Task<Commodity> GetById(Guid id)
        {
            return await _commodityRepository.GetById(id);
        }

        public async Task<Commodity> Add(Commodity commodity)
        {
            if (_commodityRepository.Search(c => c.Name == commodity.Name && c.SpecificationId == commodity.SpecificationId).Result.Any())
                return null;

            await _commodityRepository.Add(commodity);
            return commodity;
        }

        public async Task<Commodity> Update(Commodity commodity)
        {
            if (_commodityRepository.Search(c => c.Name == commodity.Name && c.SpecificationId == commodity.SpecificationId && c.Id != commodity.Id).Result.Any())
                return null;

            await _commodityRepository.Update(commodity);
            return commodity;
        }

        public async Task<bool> Remove(Commodity commodity)
        {
            await _commodityRepository.Remove(commodity);
            return true;
        }

        public async Task<IEnumerable<Commodity>> Search(string searchCriteria)
        {
            return await _commodityRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _commodityRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _commodityRepository.HasDependencies(id);
        }
    }
}