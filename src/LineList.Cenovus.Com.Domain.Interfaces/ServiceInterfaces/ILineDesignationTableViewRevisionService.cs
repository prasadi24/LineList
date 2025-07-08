using LineList.Cenovus.Com.Domain.Models;

public interface ILineDesignationTableViewRevisionService
{
    Task<IEnumerable<LineDesignationTableViewRevision>> GetAll();

    Task<LineDesignationTableViewRevision> GetById(Guid id);

    Task<LineDesignationTableViewRevision> Add(LineDesignationTableViewRevision entity);

    Task<LineDesignationTableViewRevision> Update(LineDesignationTableViewRevision entity);

    Task<bool> Remove(LineDesignationTableViewRevision entity);

    Task<IEnumerable<LineDesignationTableViewRevision>> Search(string searchCriteria);
}