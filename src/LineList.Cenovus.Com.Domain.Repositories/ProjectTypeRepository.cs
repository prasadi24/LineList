using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class ProjectTypeRepository : Repository<ProjectType>, IProjectTypeRepository
{
    public ProjectTypeRepository(LineListDbContext context) : base(context)
    {
    }

    public bool HasDependencies(Guid id)
    {
        return Db.CenovusProjects.Any(m => m.ProjectTypeId == id);
    }
}