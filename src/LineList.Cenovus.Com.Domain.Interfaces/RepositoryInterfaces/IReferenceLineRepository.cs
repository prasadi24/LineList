using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IReferenceLineRepository : IRepository<ReferenceLine>
{
    new Task<List<ReferenceLine>> GetAll();

    new Task<ReferenceLine> GetById(Guid id);
}