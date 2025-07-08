using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class PaintSystemService : IPaintSystemService
    {
        private readonly IPaintSystemRepository _paintSystemRepository;

        public PaintSystemService(IPaintSystemRepository paintSystemRepository)
        {
            _paintSystemRepository = paintSystemRepository;
        }

        public async Task<IEnumerable<PaintSystem>> GetAll()
        {
            return await _paintSystemRepository.GetAll();
        }

        public async Task<PaintSystem> GetById(Guid id)
        {
            return await _paintSystemRepository.GetById(id);
        }

        public async Task<PaintSystem> Add(PaintSystem paintSystem)
        {
            if (_paintSystemRepository.Search(c => c.Name == paintSystem.Name).Result.Any())
                return null;

            await _paintSystemRepository.Add(paintSystem);
            return paintSystem;
        }

        public async Task<PaintSystem> Update(PaintSystem paintSystem)
        {
            if (_paintSystemRepository.Search(c => c.Name == paintSystem.Name && c.Id != paintSystem.Id).Result.Any())
                return null;

            await _paintSystemRepository.Update(paintSystem);
            return paintSystem;
        }

        public async Task<bool> Remove(PaintSystem paintSystem)
        {
            await _paintSystemRepository.Remove(paintSystem);
            return true;
        }

        public async Task<IEnumerable<PaintSystem>> Search(string searchCriteria)
        {
            return await _paintSystemRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _paintSystemRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _paintSystemRepository.HasDependencies(id);
        }
    }
}