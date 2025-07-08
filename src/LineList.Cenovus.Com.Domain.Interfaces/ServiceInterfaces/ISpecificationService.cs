using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ISpecificationService
    {
        Task<IEnumerable<Specification>> GetAll();

        Task<Specification> GetById(Guid id);

        Task<Specification> Add(Specification specification);

        Task<Specification> Update(Specification specification);

        Task<bool> Remove(Specification specification);

        Task<IEnumerable<Specification>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}