using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IPressureProtectionRepository : IRepository<PressureProtection>
{
    new Task<List<PressureProtection>> GetAll();

    new Task<PressureProtection> GetById(Guid id);

    bool HasDependencies(Guid id);
}