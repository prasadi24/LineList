using LineList.Cenovus.Com.Domain.Models;


public interface ISegmentTypeService
{
    Task<IEnumerable<SegmentType>> GetAll();

    Task<SegmentType> GetById(Guid id);

    Task<SegmentType> Add(SegmentType segmentType);

    Task<SegmentType> Update(SegmentType segmentType);

    Task<bool> Remove(SegmentType segmentType);

    Task<IEnumerable<SegmentType>> Search(string searchCriteria);

    bool HasDependencies(Guid id);
}