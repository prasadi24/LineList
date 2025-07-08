using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class NdeCategoryService : INdeCategoryService
    {
        private readonly INdeCategoryRepository _ndeCategoryRepository;

        public NdeCategoryService(INdeCategoryRepository ndeCategoryRepository)
        {
            _ndeCategoryRepository = ndeCategoryRepository;
        }

        public async Task<IEnumerable<NdeCategory>> GetAll()
        {
            return await _ndeCategoryRepository.GetAll();
        }

        public async Task<NdeCategory> GetById(Guid id)
        {
            return await _ndeCategoryRepository.GetById(id);
        }

        public async Task<NdeCategory> Add(NdeCategory ndeCategory)
        {
            if (_ndeCategoryRepository.Search(c => c.Name == ndeCategory.Name).Result.Any())
                return null;

            await _ndeCategoryRepository.Add(ndeCategory);
            return ndeCategory;
        }

        public async Task<NdeCategory> Update(NdeCategory ndeCategory)
        {
            if (_ndeCategoryRepository.Search(c => c.Name == ndeCategory.Name && c.Id != ndeCategory.Id).Result.Any())
                return null;

            await _ndeCategoryRepository.Update(ndeCategory);
            return ndeCategory;
        }

        public async Task<bool> Remove(NdeCategory ndeCategory)
        {
            await _ndeCategoryRepository.Remove(ndeCategory);
            return true;
        }

        public async Task<IEnumerable<NdeCategory>> Search(string searchCriteria)
        {
            return await _ndeCategoryRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _ndeCategoryRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _ndeCategoryRepository.HasDependencies(id);
        }
    }
}