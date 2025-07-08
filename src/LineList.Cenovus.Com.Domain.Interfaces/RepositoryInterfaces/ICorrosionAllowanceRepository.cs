using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ICorrosionAllowanceRepository : IRepository<CorrosionAllowance>
    {
        new Task<List<CorrosionAllowance>> GetAll();

        new Task<CorrosionAllowance> GetById(Guid id);

        bool HasDependencies(Guid id);
    }
}