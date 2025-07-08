using LineList.Cenovus.Com.Domain.Models;

namespace LineList.Cenovus.Com.Domain.Interfaces.ServiceInterfaces
{
    public interface IConcurrentEngineeringLineService
    {
        Task<IEnumerable<ConcurrentEngineeringLine>> GetAll();

        Task<ConcurrentEngineeringLine> GetById(Guid id);

        Task<ConcurrentEngineeringLine> Add(ConcurrentEngineeringLine concurrentEngineeringLine);

        Task<ConcurrentEngineeringLine> Update(ConcurrentEngineeringLine concurrentEngineeringLine);

        Task<bool> Remove(ConcurrentEngineeringLine concurrentEngineeringLine);

        Task<IEnumerable<ConcurrentEngineeringLine>> GetFilteredLines(Guid facilityId, Guid projectId, DateTime? ldtFromDate, DateTime? ldtToDate, bool showAsBuilt);

        //Task<IEnumerable<ConcurrentEngineeringLine>> Search(string searchCriteria);
    }
}