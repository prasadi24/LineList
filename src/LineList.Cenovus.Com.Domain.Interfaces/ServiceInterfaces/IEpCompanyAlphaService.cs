using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IEpCompanyAlphaService
    {
        Task<IEnumerable<EpCompanyAlpha>> GetAll();

        Task<EpCompanyAlpha> GetById(Guid id);

        Task<EpCompanyAlpha> Add(EpCompanyAlpha epCompanyAlpha);

        Task<EpCompanyAlpha> Update(EpCompanyAlpha epCompanyAlpha);

        Task<bool> Remove(EpCompanyAlpha epCompanyAlpha);

        Task<IEnumerable<EpCompanyAlpha>> Search(string searchCriteria);
    }
}