using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class InsulationThicknessService : IInsulationThicknessService
    {
        private readonly IInsulationThicknessRepository _insulationThicknessRepository;

        public InsulationThicknessService(IInsulationThicknessRepository insulationThicknessRepository)
        {
            _insulationThicknessRepository = insulationThicknessRepository;
        }

        public async Task<IEnumerable<InsulationThickness>> GetAll()
        {
            return await _insulationThicknessRepository.GetAll();
        }

        public async Task<InsulationThickness> GetById(Guid id)
        {
            return await _insulationThicknessRepository.GetById(id);
        }

        public async Task<InsulationThickness> Add(InsulationThickness insulationThickness)
        {
            if (_insulationThicknessRepository.Search(c => c.Name == insulationThickness.Name).Result.Any())
                return null;

            await _insulationThicknessRepository.Add(insulationThickness);
            return insulationThickness;
        }

        public async Task<InsulationThickness> Update(InsulationThickness insulationThickness)
        {
            if (_insulationThicknessRepository.Search(c => c.Name == insulationThickness.Name && c.Id != insulationThickness.Id).Result.Any())
                return null;

            await _insulationThicknessRepository.Update(insulationThickness);
            return insulationThickness;
        }

        public async Task<bool> Remove(InsulationThickness insulationThickness)
        {
            await _insulationThicknessRepository.Remove(insulationThickness);
            return true;
        }

        public async Task<IEnumerable<InsulationThickness>> Search(string searchCriteria)
        {
            return await _insulationThicknessRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _insulationThicknessRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return _insulationThicknessRepository.HasDependencies(id);
        }
    }
}