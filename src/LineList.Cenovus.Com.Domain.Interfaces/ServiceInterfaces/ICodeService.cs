using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ICodeService
    {
        Task<IEnumerable<Code>> GetAll();

        Task<Code> GetById(Guid id);

        Task<Code> Add(Code code);

        Task<Code> Update(Code code);

        Task<bool> Remove(Code code);

        Task<IEnumerable<Code>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}