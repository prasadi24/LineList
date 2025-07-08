using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IFluidRepository : IRepository<Fluid>
    {
        new Task<List<Fluid>> GetAll();

        new Task<Fluid> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}