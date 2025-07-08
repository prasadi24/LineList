using LineList.Cenovus.Com.Domain.Models;
using LineList.Cenovus.Com.Domain.Repositories;
using LineList.Cenovus.Com.Infrastructure.Context;

public class PostWeldHeatTreatmentRepository : Repository<PostWeldHeatTreatment>, IPostWeldHeatTreatmentRepository
{
    public PostWeldHeatTreatmentRepository(LineListDbContext context) : base(context)
    {
    }

    public bool HasDependencies(Guid id)
    {
        return Db.LineRevisions.Any(m => m.PostWeldHeatTreatmentId == id)
             || Db.PipeSpecifications.Any(m => m.PostWeldHeatTreatmentId == id);
    }
}