using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface ISegmentTypeRepository : IRepository<SegmentType>
{
    new Task<List<SegmentType>> GetAll();

    new Task<SegmentType> GetById(Guid id);

    bool HasDependencies(Guid id);
}