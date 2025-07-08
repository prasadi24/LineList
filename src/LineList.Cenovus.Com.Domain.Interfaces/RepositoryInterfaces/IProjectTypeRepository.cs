using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IProjectTypeRepository : IRepository<ProjectType>
{
    new Task<List<ProjectType>> GetAll();

    new Task<ProjectType> GetById(Guid id);

    bool HasDependencies(Guid id);
}