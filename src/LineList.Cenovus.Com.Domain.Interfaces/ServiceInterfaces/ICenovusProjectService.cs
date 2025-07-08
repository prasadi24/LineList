using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ICenovusProjectService
    {
        Task<IEnumerable<CenovusProject>> GetAll();

        Task<CenovusProject> GetById(Guid id);

        Task<CenovusProject> Add(CenovusProject cenovusProject);

        Task<CenovusProject> Update(CenovusProject cenovusProject);

        Task<bool> Remove(CenovusProject cenovusProject);

        Task<IEnumerable<CenovusProject>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}