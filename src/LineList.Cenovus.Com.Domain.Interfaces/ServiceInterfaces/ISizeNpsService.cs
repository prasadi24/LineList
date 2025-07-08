using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ISizeNpsService
    {
        Task<IEnumerable<SizeNps>> GetAll();

        Task<SizeNps> GetById(Guid id);

        Task<SizeNps> Add(SizeNps sizeNps);

        Task<SizeNps> Update(SizeNps sizeNps);

        Task<bool> Remove(SizeNps sizeNps);

        Task<IEnumerable<SizeNps>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}