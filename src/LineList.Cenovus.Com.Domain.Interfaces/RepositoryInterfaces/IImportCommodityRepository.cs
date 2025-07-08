using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportCommodityRepository : IRepository<ImportCommodity>
    {
        new Task<List<ImportCommodity>> GetAll();

        new Task<ImportCommodity> GetById(Guid id);
    }
}