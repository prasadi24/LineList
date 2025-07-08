using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface ILineDesignationTableViewHeaderRepository : IRepository<LineDesignationTableViewHeader>
    {
        new Task<List<LineDesignationTableViewHeader>> GetAll();

        new Task<LineDesignationTableViewHeader> GetById(Guid id);
    }
}