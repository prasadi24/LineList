using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IInsulationTypeRepository : IRepository<InsulationType>
{
    new Task<List<InsulationType>> GetAll();

    new Task<InsulationType> GetById(Guid id);

    bool HasDependencies(Guid id);
}