using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IEpCompanyAlphaRepository : IRepository<EpCompanyAlpha>
    {
        new Task<List<EpCompanyAlpha>> GetAll();

        new Task<EpCompanyAlpha> GetById(Guid id);
    }
}