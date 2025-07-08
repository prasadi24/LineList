using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ILLLookupTableNoDescriptionService
    {
        Task<IEnumerable<LLLookupTableNoDescription>> GetAll();

        Task<LLLookupTableNoDescription> GetById(Guid id);

        Task<LLLookupTableNoDescription> Add(LLLookupTableNoDescription lookupTable);

        Task<LLLookupTableNoDescription> Update(LLLookupTableNoDescription lookupTable);

        Task<bool> Remove(LLLookupTableNoDescription lookupTable);

        Task<IEnumerable<LLLookupTableNoDescription>> Search(string searchCriteria);
    }
}