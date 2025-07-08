using LineList.Cenovus.Com.Domain.Models;

public interface IReferenceLineService
{
    Task<IEnumerable<ReferenceLine>> GetAll();

    Task<ReferenceLine> GetById(Guid id);

    Task<ReferenceLine> Add(ReferenceLine referenceLine);

    Task<ReferenceLine> Update(ReferenceLine referenceLine);

    Task<bool> Remove(ReferenceLine referenceLine);

    Task<IEnumerable<ReferenceLine>> Search(string searchCriteria);
}