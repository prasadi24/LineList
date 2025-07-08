using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILLLookupTableNoDescriptionRepository : IRepository<LLLookupTableNoDescription>
    {
        new Task<List<LLLookupTableNoDescription>> GetAll();

        new Task<LLLookupTableNoDescription> GetById(Guid id);
    }
}