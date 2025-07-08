using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ICommodityService
    {
        Task<IEnumerable<Commodity>> GetAll();

        Task<Commodity> GetById(Guid id);

        Task<Commodity> Add(Commodity commodity);

        Task<Commodity> Update(Commodity commodity);

        Task<bool> Remove(Commodity commodity);

        Task<IEnumerable<Commodity>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}