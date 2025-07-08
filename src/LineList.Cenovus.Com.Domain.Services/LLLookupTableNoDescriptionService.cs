using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LLLookupTableNoDescriptionService : ILLLookupTableNoDescriptionService
    {
        private readonly ILLLookupTableNoDescriptionRepository _lookupTableRepository;

        public LLLookupTableNoDescriptionService(ILLLookupTableNoDescriptionRepository lookupTableRepository)
        {
            _lookupTableRepository = lookupTableRepository;
        }

        public async Task<IEnumerable<LLLookupTableNoDescription>> GetAll()
        {
            return await _lookupTableRepository.GetAll();
        }

        public async Task<LLLookupTableNoDescription> GetById(Guid id)
        {
            return await _lookupTableRepository.GetById(id);
        }

        public async Task<LLLookupTableNoDescription> Add(LLLookupTableNoDescription lookupTable)
        {
            // Check if a lookup table with the same value already exists
            if (_lookupTableRepository.Search(c => c.Name == lookupTable.Name).Result.Any())
                return null;

            await _lookupTableRepository.Add(lookupTable);
            return lookupTable;
        }

        public async Task<LLLookupTableNoDescription> Update(LLLookupTableNoDescription lookupTable)
        {
            // Ensure no other lookup table with the same value exists
            if (_lookupTableRepository.Search(c => c.Name == lookupTable.Name && c.Id != lookupTable.Id).Result.Any())
                return null;

            await _lookupTableRepository.Update(lookupTable);
            return lookupTable;
        }

        public async Task<bool> Remove(LLLookupTableNoDescription lookupTable)
        {
            await _lookupTableRepository.Remove(lookupTable);
            return true;
        }

        public async Task<IEnumerable<LLLookupTableNoDescription>> Search(string searchCriteria)
        {
            return await _lookupTableRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _lookupTableRepository?.Dispose();
        }
    }
}