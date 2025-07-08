using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpCompanyRepository : IRepository<EpCompany>
    {
        new Task<List<EpCompany>> GetAll();

        new Task<EpCompany> GetById(Guid id);

        Task<bool> UpdateCompanyLogo(Guid companyId, string base64String);

        bool HasDependencies(Guid id);
    }
}