using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILLLookupTableRepository : IRepository<LLLookupTable>
    {
        new Task<List<LLLookupTable>> GetAll();

        new Task<LLLookupTable> GetById(Guid id);
    }
}