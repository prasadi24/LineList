using LineList.Cenovus.Com.Domain.Interfaces;
using LineList.Cenovus.Com.Domain.Models;

public interface ILineDesignationTableViewRevisionRepository : IRepository<LineDesignationTableViewRevision>
{
    new Task<List<LineDesignationTableViewRevision>> GetAll();

    new Task<LineDesignationTableViewRevision> GetById(Guid id);
}