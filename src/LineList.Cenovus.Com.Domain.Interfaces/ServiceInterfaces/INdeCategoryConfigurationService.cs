using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface INdeCategoryService
    {
        Task<IEnumerable<NdeCategory>> GetAll();

        Task<NdeCategory> GetById(Guid id);

        Task<NdeCategory> Add(NdeCategory ndeCategory);

        Task<NdeCategory> Update(NdeCategory ndeCategory);

        Task<bool> Remove(NdeCategory ndeCategory);

        Task<IEnumerable<NdeCategory>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}