using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface IPostWeldHeatTreatmentRepository : IRepository<PostWeldHeatTreatment>
{
    new Task<List<PostWeldHeatTreatment>> GetAll();

    new Task<PostWeldHeatTreatment> GetById(Guid id);

    bool HasDependencies(Guid id);
}