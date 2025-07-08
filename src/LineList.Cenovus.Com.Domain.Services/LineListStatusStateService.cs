using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LineListStatusStateService : ILineListStatusStateService
    {
        private readonly ILineListStatusStateRepository _LineListStatusStateRepository;

        public LineListStatusStateService(ILineListStatusStateRepository LineListStatusStateRepository)
        {
            _LineListStatusStateRepository = LineListStatusStateRepository;
        }

        public async Task<IEnumerable<LineListStatusState>> GetAll()
        {
            return await _LineListStatusStateRepository.GetAll();
        }

        public async Task<LineListStatusState> GetById(Guid id)
        {
            return await _LineListStatusStateRepository.GetById(id);
        }

        public async Task<LineListStatusState> Add(LineListStatusState LineListStatusState)
        {
            // Prevent adding a duplicate LineListStatusState entry based on Name
            if (_LineListStatusStateRepository.Search(c => c.Id == LineListStatusState.Id).Result.Any())
                return null;

            await _LineListStatusStateRepository.Add(LineListStatusState);
            return LineListStatusState;
        }

        public async Task<LineListStatusState> Update(LineListStatusState LineListStatusState)
        {
            // Prevent updating to a duplicate LineListStatusState entry based on Name
            if (_LineListStatusStateRepository.Search(c => c.Id == LineListStatusState.Id && c.Id != LineListStatusState.Id).Result.Any())
                return null;

            await _LineListStatusStateRepository.Update(LineListStatusState);
            return LineListStatusState;
        }

        public async Task<bool> Remove(LineListStatusState LineListStatusState)
        {
            await _LineListStatusStateRepository.Remove(LineListStatusState);
            return true;
        }

        //public async Task<IEnumerable<LineListStatusState>> Search(string searchCriteria)
        //{
        //    return await _LineListStatusStateRepository.Search(c => c.Id.Contains(searchCriteria));
        //}

        public void Dispose()
        {
            _LineListStatusStateRepository?.Dispose();
        }
    }
}