using LineList.Cenovus.Com.Domain.Models;


namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpCompanyService
    {
        Task<IEnumerable<EpCompany>> GetAll();

        Task<EpCompany> GetById(Guid id);

        Task<EpCompany> Add(EpCompany epCompany);

        Task<EpCompany> Update(EpCompany epCompany);

        Task<bool> Remove(EpCompany epCompany);

        Task<IEnumerable<EpCompany>> Search(string searchCriteria);

        Task<bool> UpdateCompanyLogo(Guid companyId, string base64String);

        bool HasDependencies(Guid id);
    }
}