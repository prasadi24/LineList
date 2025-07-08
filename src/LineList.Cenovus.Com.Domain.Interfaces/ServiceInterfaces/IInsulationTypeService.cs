using LineList.Cenovus.Com.Domain.Models;

public interface IInsulationTypeService
{
    Task<IEnumerable<InsulationType>> GetAll();

    Task<InsulationType> GetById(Guid id);

    Task<InsulationType> Add(InsulationType insulationType);

    Task<InsulationType> Update(InsulationType insulationType);

    Task<bool> Remove(InsulationType insulationType);

    Task<IEnumerable<InsulationType>> Search(string searchCriteria);

    bool HasDependencies(Guid id);
}