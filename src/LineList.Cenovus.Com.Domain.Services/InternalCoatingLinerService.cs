using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class InternalCoatingLinerService : IInternalCoatingLinerService
    {
        private readonly IInternalCoatingLinerRepository _internalCoatingLinerRepository;

        public InternalCoatingLinerService(IInternalCoatingLinerRepository internalCoatingLinerRepository)
        {
            _internalCoatingLinerRepository = internalCoatingLinerRepository;
        }

        public async Task<IEnumerable<InternalCoatingLiner>> GetAll()
        {
            return await _internalCoatingLinerRepository.GetAll();
        }

        public async Task<InternalCoatingLiner> GetById(Guid id)
        {
            return await _internalCoatingLinerRepository.GetById(id);
        }

        public async Task<InternalCoatingLiner> Add(InternalCoatingLiner internalCoatingLiner)
        {
            if (_internalCoatingLinerRepository.Search(c => c.Name == internalCoatingLiner.Name).Result.Any())
                return null;

            await _internalCoatingLinerRepository.Add(internalCoatingLiner);
            return internalCoatingLiner;
        }

        public async Task<InternalCoatingLiner> Update(InternalCoatingLiner internalCoatingLiner)
        {
            if (_internalCoatingLinerRepository.Search(c => c.Name == internalCoatingLiner.Name && c.Id != internalCoatingLiner.Id).Result.Any())
                return null;

            await _internalCoatingLinerRepository.Update(internalCoatingLiner);
            return internalCoatingLiner;
        }

        public async Task<bool> Remove(InternalCoatingLiner internalCoatingLiner)
        {
            await _internalCoatingLinerRepository.Remove(internalCoatingLiner);
            return true;
        }

        public async Task<IEnumerable<InternalCoatingLiner>> Search(string searchCriteria)
        {
            return await _internalCoatingLinerRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _internalCoatingLinerRepository?.Dispose();
        }

        public bool HasDependencies(Guid id)
        {
            return  _internalCoatingLinerRepository.HasDependencies(id);
        }
    }
}