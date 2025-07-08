using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.RepositoryInterfaces
{
    public interface IConcurrentEngineeringLineRepository : IRepository<ConcurrentEngineeringLine>
    {
        new Task<List<ConcurrentEngineeringLine>> GetAll();

        new Task<ConcurrentEngineeringLine> GetById(Guid id);

        Task<IEnumerable<ConcurrentEngineeringLine>> GetAllLines();
        IQueryable<ConcurrentEngineeringLine> GetAllLinesQuery();
    }
}