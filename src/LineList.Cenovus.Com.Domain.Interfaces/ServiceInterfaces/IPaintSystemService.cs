using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IPaintSystemService
    {
        Task<IEnumerable<PaintSystem>> GetAll();

        Task<PaintSystem> GetById(Guid id);

        Task<PaintSystem> Add(PaintSystem paintSystem);

        Task<PaintSystem> Update(PaintSystem paintSystem);

        Task<bool> Remove(PaintSystem paintSystem);

        Task<IEnumerable<PaintSystem>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}