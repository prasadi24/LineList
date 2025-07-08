using LineList.Cenovus.Com.Domain.Models;


public interface IProjectTypeService
{
    Task<IEnumerable<ProjectType>> GetAll();

    Task<ProjectType> GetById(Guid id);

    Task<ProjectType> Add(ProjectType projectType);

    Task<ProjectType> Update(ProjectType projectType);

    Task<bool> Remove(ProjectType projectType);

    Task<IEnumerable<ProjectType>> Search(string searchCriteria);

    bool HasDependencies(Guid id);
}