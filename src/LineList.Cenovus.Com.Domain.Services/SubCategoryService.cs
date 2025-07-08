using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;

        public SubCategoryService(ISubCategoryRepository subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<IEnumerable<SubCategory>> GetAll()
        {
            return await _subCategoryRepository.GetAll();
        }

        public async Task<SubCategory> GetById(int id)
        {
            return await _subCategoryRepository.GetById(id);
        }

        public async Task<SubCategory> Add(SubCategory category)
        {
            if (_subCategoryRepository.Search(c => c.Name == category.Name).Result.Any())
                return null;

            await _subCategoryRepository.Add(category);
            return category;
        }

        public async Task<SubCategory> Update(SubCategory category)
        {
            if (_subCategoryRepository.Search(c => c.Name == category.Name && c.ID != category.ID).Result.Any())
                return null;

            await _subCategoryRepository.Update(category);
            return category;
        }

        public async Task<bool> Remove(SubCategory category)
        {
            await _subCategoryRepository.Remove(category);
            return true;
        }

        public async Task<IEnumerable<SubCategory>> Search(string categoryName)
        {
            return await _subCategoryRepository.Search(c => c.Name.Contains(categoryName));
        }
        public async Task<IEnumerable<SubCategory>> SearchSubCategory(string categoryName)
        {
            return await _subCategoryRepository.SearchSubCategory(categoryName);
        }
        public void Dispose()
        {
            _subCategoryRepository?.Dispose();
        }
    }
}