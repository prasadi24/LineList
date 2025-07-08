using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILineListStatusStateRepository : IRepository<LineListStatusState>
    {
        new Task<List<LineListStatusState>> GetAll();

        new Task<LineListStatusState> GetById(Guid id);
    }
}