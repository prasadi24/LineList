using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ICorrosionAllowanceService
    {
        Task<IEnumerable<CorrosionAllowance>> GetAll();

        Task<CorrosionAllowance> GetById(Guid id);

        Task<CorrosionAllowance> Add(CorrosionAllowance corrosionAllowance);

        Task<CorrosionAllowance> Update(CorrosionAllowance corrosionAllowance);

        Task<bool> Remove(CorrosionAllowance corrosionAllowance);

        Task<IEnumerable<CorrosionAllowance>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}