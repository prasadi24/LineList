using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IImportRowViewRepository : IRepository<ImportRowView>
    {
        new Task<List<ImportRowView>> GetAll();

        new Task<ImportRowView> GetById(Guid id);
    }
}