using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ICsaClassLocationService
    {
        Task<IEnumerable<CsaClassLocation>> GetAll();

        Task<CsaClassLocation> GetById(Guid id);

        Task<CsaClassLocation> Add(CsaClassLocation csaClassLocation);

        Task<CsaClassLocation> Update(CsaClassLocation csaClassLocation);

        Task<bool> Remove(CsaClassLocation csaClassLocation);

        Task<IEnumerable<CsaClassLocation>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}