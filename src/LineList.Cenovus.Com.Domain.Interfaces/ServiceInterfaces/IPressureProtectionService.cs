using LineList.Cenovus.Com.Domain.Models;

public interface IPressureProtectionService
{
    Task<IEnumerable<PressureProtection>> GetAll();

    Task<PressureProtection> GetById(Guid id);

    Task<PressureProtection> Add(PressureProtection pressureProtection);

    Task<PressureProtection> Update(PressureProtection pressureProtection);

    Task<bool> Remove(PressureProtection pressureProtection);

    Task<IEnumerable<PressureProtection>> Search(string searchCriteria);

    bool HasDependencies(Guid id);
}