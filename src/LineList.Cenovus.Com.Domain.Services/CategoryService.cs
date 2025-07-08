using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _categoryRepository.GetAll();
        }

        public async Task<Category> GetById(int id)
        {
            return await _categoryRepository.GetById(id);
        }

        public async Task<Category> Add(Category category)
        {
            if (_categoryRepository.Search(c => c.Name == category.Name).Result.Any())
                return null;

            await _categoryRepository.Add(category);
            return category;
        }

        public async Task<Category> Update(Category category)
        {
            if (_categoryRepository.Search(c => c.Name == category.Name && c.ID != category.ID).Result.Any())
                return null;

            await _categoryRepository.Update(category);
            return category;
        }

        public async Task<bool> Remove(Category category)
        {
            await _categoryRepository.Remove(category);
            return true;
        }

        public async Task<IEnumerable<Category>> Search(string categoryName)
        {
            return await _categoryRepository.Search(c => c.Name.Contains(categoryName));
        }

        public void Dispose()
        {
            _categoryRepository?.Dispose();
        }
    }
}