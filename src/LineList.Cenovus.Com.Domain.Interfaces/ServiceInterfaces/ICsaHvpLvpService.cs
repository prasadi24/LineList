using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface ICsaHvpLvpService
    {
        Task<IEnumerable<CsaHvpLvp>> GetAll();

        Task<CsaHvpLvp> GetById(Guid id);

        Task<CsaHvpLvp> Add(CsaHvpLvp csaHvpLvp);

        Task<CsaHvpLvp> Update(CsaHvpLvp csaHvpLvp);

        Task<bool> Remove(CsaHvpLvp csaHvpLvp);

        Task<IEnumerable<CsaHvpLvp>> Search(string searchCriteria);

        bool HasDependencies(Guid id);
    }
}