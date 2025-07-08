using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;

        public AreaService(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        public async Task<IEnumerable<Area>> GetAll()
        {
            return await _areaRepository.GetAll();
        }

        public async Task<Area> GetById(Guid id)
        {
            return await _areaRepository.GetById(id);
        }

        public async Task<Area> Add(Area area)
        {
            if (_areaRepository.Search(a => a.Name == area.Name && a.LocationId==area.LocationId).Result.Any())
                return null;

            await _areaRepository.Add(area);
            return area;
        }

        public async Task<Area> Update(Area area)
        {
            if (_areaRepository.Search(a => a.Name == area.Name && a.LocationId == area.LocationId && a.Id != area.Id).Result.Any())
                //if (_areaRepository.Search(a => a.Name == area.Name && a.Id != area.Id).Result.Any())
                return null;

            await _areaRepository.Update(area);
            return area;
        }

        public async Task<bool> Remove(Area area)
        {
            await _areaRepository.Remove(area);
            return true;
        }

        public async Task<IEnumerable<Area>> Search(string searchCriteria)
        {
            return await _areaRepository.Search(a => a.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _areaRepository?.Dispose();
        }

        public bool HasDependencies(Guid areaId)
        {
            return _areaRepository.HasDependencies(areaId);
        }
    }
}