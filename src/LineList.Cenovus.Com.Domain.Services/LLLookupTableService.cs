using LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces;
using LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces;
using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Services
{
    public class LLLookupTableService : ILLLookupTableService
    {
        private readonly ILLLookupTableRepository _lookupTableRepository;

        public LLLookupTableService(ILLLookupTableRepository lookupTableRepository)
        {
            _lookupTableRepository = lookupTableRepository;
        }

        public async Task<IEnumerable<LLLookupTable>> GetAll()
        {
            return await _lookupTableRepository.GetAll();
        }

        public async Task<LLLookupTable> GetById(Guid id)
        {
            return await _lookupTableRepository.GetById(id);
        }

        public async Task<LLLookupTable> Add(LLLookupTable lookupTable)
        {
            // Check if a lookup table with the same value already exists
            if (_lookupTableRepository.Search(c => c.Name == lookupTable.Name).Result.Any())
                return null;

            await _lookupTableRepository.Add(lookupTable);
            return lookupTable;
        }

        public async Task<LLLookupTable> Update(LLLookupTable lookupTable)
        {
            // Ensure no other lookup table with the same value exists
            if (_lookupTableRepository.Search(c => c.Name == lookupTable.Name && c.Id != lookupTable.Id).Result.Any())
                return null;

            await _lookupTableRepository.Update(lookupTable);
            return lookupTable;
        }

        public async Task<bool> Remove(LLLookupTable lookupTable)
        {
            await _lookupTableRepository.Remove(lookupTable);
            return true;
        }

        public async Task<IEnumerable<LLLookupTable>> Search(string searchCriteria)
        {
            return await _lookupTableRepository.Search(c => c.Name.Contains(searchCriteria));
        }

        public void Dispose()
        {
            _lookupTableRepository?.Dispose();
        }
    }
}