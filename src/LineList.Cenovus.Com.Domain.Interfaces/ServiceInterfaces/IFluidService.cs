using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IFluidService
    {
        Task<IEnumerable<Fluid>> GetAll();

        Task<Fluid> GetById(Guid id);

        Task<Fluid> Add(Fluid fluid);

        Task<Fluid> Update(Fluid fluid);

        Task<bool> Remove(Fluid fluid);

        Task<IEnumerable<Fluid>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}