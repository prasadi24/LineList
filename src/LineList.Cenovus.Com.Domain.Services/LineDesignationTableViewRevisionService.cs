using LineList.Cenovus.Com.Domain.Models;

public class LineDesignationTableViewRevisionService : ILineDesignationTableViewRevisionService
{
    private readonly ILineDesignationTableViewRevisionRepository _repository;

    public LineDesignationTableViewRevisionService(ILineDesignationTableViewRevisionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<LineDesignationTableViewRevision>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<LineDesignationTableViewRevision> GetById(Guid id)
    {
        return await _repository.GetById(id);
    }

    public async Task<LineDesignationTableViewRevision> Add(LineDesignationTableViewRevision entity)
    {
        await _repository.Add(entity);
        return entity;
    }

    public async Task<LineDesignationTableViewRevision> Update(LineDesignationTableViewRevision entity)
    {
        await _repository.Update(entity);
        return entity;
    }

    public async Task<bool> Remove(LineDesignationTableViewRevision entity)
    {
        await _repository.Remove(entity);
        return true;
    }

    public async Task<IEnumerable<LineDesignationTableViewRevision>> Search(string searchCriteria)
    {
        return await _repository.Search(c => c.Description.Contains(searchCriteria));
    }

    public void Dispose()
    {
        _repository?.Dispose();
    }
}