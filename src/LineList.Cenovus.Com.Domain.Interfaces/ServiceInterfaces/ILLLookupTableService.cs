using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILLLookupTableService
    {
        Task<IEnumerable<LLLookupTable>> GetAll();

        Task<LLLookupTable> GetById(Guid id);

        Task<LLLookupTable> Add(LLLookupTable lookupTable);

        Task<LLLookupTable> Update(LLLookupTable lookupTable);

        Task<bool> Remove(LLLookupTable lookupTable);

        Task<IEnumerable<LLLookupTable>> Search(string searchCriteria);
    }
}