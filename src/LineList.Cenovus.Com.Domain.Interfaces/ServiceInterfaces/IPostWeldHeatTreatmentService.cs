using LineList.Cenovus.Com.Domain.Models;


public interface IPostWeldHeatTreatmentService
{
    Task<IEnumerable<PostWeldHeatTreatment>> GetAll();

    Task<PostWeldHeatTreatment> GetById(Guid id);

    Task<PostWeldHeatTreatment> Add(PostWeldHeatTreatment postWeldHeatTreatment);

    Task<PostWeldHeatTreatment> Update(PostWeldHeatTreatment postWeldHeatTreatment);

    Task<bool> Remove(PostWeldHeatTreatment postWeldHeatTreatment);

    Task<IEnumerable<PostWeldHeatTreatment>> Search(string searchCriteria);

    bool HasDependencies(Guid id);
}